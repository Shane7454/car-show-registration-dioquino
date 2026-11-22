namespace Application.Features.Registrations.GetRegistrationById;

using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

public sealed class GetRegistrationByIdHandler(IAppDbContext dbContext)
    : IQueryHandler<GetRegistrationByIdQuery, Result<GetRegistrationByIdResponse>>
{
    public async Task<Result<GetRegistrationByIdResponse>> HandleAsync(
        GetRegistrationByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var registration = await dbContext.Registrations
            .AsNoTracking()
            .Where(x => x.Id == query.Id)
            .Select(x => new GetRegistrationByIdResponse(
                x.Id,
                x.RegistrationNumber,
                x.RegistrationDate,
                x.ParticipantName,
                x.ContactNumber,
                x.Email,
                x.Address,
                x.CarMakeModel,
                x.PlateNumber,
                x.RegistrationType,
                x.Status,
                x.RegistrationFee,
                x.AmountPaid,
                x.AmountPaid == 0
                    ? PaymentStatus.Unpaid
                    : x.AmountPaid < x.RegistrationFee
                        ? PaymentStatus.Partial
                        : PaymentStatus.Paid))
            .FirstOrDefaultAsync(cancellationToken);

        return registration is null
            ? Result<GetRegistrationByIdResponse>.Failure(
                Error.NotFound("Registration.NotFound", $"Registration with ID '{query.Id}' was not found."))
            : Result<GetRegistrationByIdResponse>.Success(registration);
    }
}
