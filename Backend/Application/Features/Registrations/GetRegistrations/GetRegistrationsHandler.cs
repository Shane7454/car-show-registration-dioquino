namespace Application.Features.Registrations.GetRegistrations;

using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

public sealed class GetRegistrationsHandler(IAppDbContext dbContext)
    : IQueryHandler<GetRegistrationsQuery, Result<GetRegistrationsResponse>>
{
    public async Task<Result<GetRegistrationsResponse>> HandleAsync(
        GetRegistrationsQuery query,
        CancellationToken cancellationToken = default)
    {
        var registrations = dbContext.Registrations.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Type))
        {
            var registrationType = Enum.Parse<RegistrationType>(query.Type, ignoreCase: true);
            registrations = registrations.Where(x => x.RegistrationType == registrationType);
        }

        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            var status = Enum.Parse<RegistrationStatus>(query.Status, ignoreCase: true);
            registrations = registrations.Where(x => x.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(query.ParticipantName))
        {
            var participantName = query.ParticipantName.Trim().ToLower();
            registrations = registrations.Where(x => x.ParticipantName.ToLower().Contains(participantName));
        }

        var totalCount = await registrations.CountAsync(cancellationToken);
        var offset = ((long)query.Page - 1) * query.PageSize;
        List<RegistrationListItemResponse> items = offset > int.MaxValue
            ? []
            : await registrations
                .OrderByDescending(x => x.RegistrationDate)
                .ThenByDescending(x => x.Id)
                .Skip((int)offset)
                .Take(query.PageSize)
                .Select(x => new RegistrationListItemResponse(
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
                .ToListAsync(cancellationToken);

        var totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)query.PageSize);
        return Result<GetRegistrationsResponse>.Success(new GetRegistrationsResponse(
            items,
            query.Page,
            query.PageSize,
            totalCount,
            totalPages));
    }
}
