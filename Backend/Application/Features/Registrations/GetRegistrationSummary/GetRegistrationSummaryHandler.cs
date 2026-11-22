namespace Application.Features.Registrations.GetRegistrationSummary;

using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

public sealed class GetRegistrationSummaryHandler(IAppDbContext dbContext)
    : IQueryHandler<GetRegistrationSummaryQuery, Result<GetRegistrationSummaryResponse>>
{
    public async Task<Result<GetRegistrationSummaryResponse>> HandleAsync(
        GetRegistrationSummaryQuery query,
        CancellationToken cancellationToken = default)
    {
        var registrations = await dbContext.Registrations
            .AsNoTracking()
            .Select(x => new RegistrationSummaryRow(x.Status, x.RegistrationFee, x.AmountPaid))
            .ToListAsync(cancellationToken);

        var nonCancelled = registrations.Where(x => x.Status != RegistrationStatus.Cancelled).ToList();
        var totalFees = nonCancelled.Sum(x => x.RegistrationFee);
        var totalPayments = nonCancelled.Sum(x => x.AmountPaid);

        return Result<GetRegistrationSummaryResponse>.Success(new GetRegistrationSummaryResponse(
            TotalRegistrations: registrations.Count,
            Confirmed: registrations.Count(x => x.Status == RegistrationStatus.Confirmed),
            CheckedIn: registrations.Count(x => x.Status == RegistrationStatus.CheckedIn),
            Pending: registrations.Count(x => x.Status == RegistrationStatus.Pending),
            Cancelled: registrations.Count(x => x.Status == RegistrationStatus.Cancelled),
            TotalFees: totalFees,
            TotalPayments: totalPayments,
            UncollectedPayments: totalFees - totalPayments));
    }

    private sealed record RegistrationSummaryRow(
        RegistrationStatus Status,
        decimal RegistrationFee,
        decimal AmountPaid);
}
