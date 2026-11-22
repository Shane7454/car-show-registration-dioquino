namespace Application.Features.Registrations.CreateRegistration;

using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

public sealed class CreateRegistrationHandler(IAppDbContext dbContext)
    : ICommandHandler<CreateRegistrationCommand, Result<CreateRegistrationResponse>>
{
    private const string RegistrationNumberPrefix = "CS27-";

    public async Task<Result<CreateRegistrationResponse>> HandleAsync(
        CreateRegistrationCommand command,
        CancellationToken cancellationToken = default)
    {
        var normalizedPlateNumber = command.PlateNumber.Trim().ToUpperInvariant();
        var duplicatePlateExists = await dbContext.Registrations
            .AnyAsync(x => x.PlateNumber == normalizedPlateNumber, cancellationToken);

        if (duplicatePlateExists)
        {
            return Result<CreateRegistrationResponse>.Failure(
                new Error("Registration.Conflict", "A registration already exists for this plate number."));
        }

        var lastRegistrationNumber = await dbContext.Registrations
            .Where(x => x.RegistrationNumber.StartsWith(RegistrationNumberPrefix))
            .OrderByDescending(x => x.RegistrationNumber)
            .Select(x => x.RegistrationNumber)
            .FirstOrDefaultAsync(cancellationToken);

        var nextNumber = 1;
        if (lastRegistrationNumber is not null &&
            int.TryParse(lastRegistrationNumber.AsSpan(RegistrationNumberPrefix.Length), out var currentNumber))
        {
            nextNumber = currentNumber + 1;
        }

        var registration = new Registration
        {
            Id = Guid.NewGuid(),
            RegistrationNumber = $"{RegistrationNumberPrefix}{nextNumber:D4}",
            RegistrationDate = DateTime.UtcNow,
            ParticipantName = command.ParticipantName.Trim(),
            ContactNumber = command.ContactNumber.Trim(),
            Email = command.Email.Trim(),
            Address = command.Address.Trim(),
            CarMakeModel = command.CarMakeModel.Trim(),
            PlateNumber = normalizedPlateNumber,
            RegistrationType = command.RegistrationType,
            Status = RegistrationStatus.Pending,
            RegistrationFee = GetFee(command.RegistrationType),
            AmountPaid = command.AmountPaid
        };

        dbContext.Registrations.Add(registration);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<CreateRegistrationResponse>.Success(new CreateRegistrationResponse(
            registration.Id,
            registration.RegistrationNumber,
            registration.RegistrationDate,
            registration.ParticipantName,
            registration.ContactNumber,
            registration.Email,
            registration.Address,
            registration.CarMakeModel,
            registration.PlateNumber,
            registration.RegistrationType,
            registration.Status,
            registration.RegistrationFee,
            registration.AmountPaid,
            registration.PaymentStatus));
    }

    private static decimal GetFee(RegistrationType type) => type switch
    {
        RegistrationType.Exhibitor => 1500m,
        RegistrationType.Competitor => 2500m,
        RegistrationType.VIP => 5000m,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unsupported registration type.")
    };
}
