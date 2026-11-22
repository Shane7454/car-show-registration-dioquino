namespace Application.Features.Registrations.GetRegistrations;

using Application.Abstractions.Messaging;
using Domain.Common;

public sealed record GetRegistrationsQuery(
    string? Type,
    string? Status,
    string? ParticipantName,
    int Page = 1,
    int PageSize = 10) : IQuery<Result<GetRegistrationsResponse>>;
