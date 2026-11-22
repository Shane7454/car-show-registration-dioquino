namespace Application.Features.Registrations.GetRegistrations;

using Domain.Enums;

public sealed record RegistrationListItemResponse(
    Guid Id,
    string RegistrationNumber,
    DateTime RegistrationDate,
    string ParticipantName,
    string ContactNumber,
    string Email,
    string Address,
    string CarMakeModel,
    string PlateNumber,
    RegistrationType RegistrationType,
    RegistrationStatus Status,
    decimal RegistrationFee,
    decimal AmountPaid,
    PaymentStatus PaymentStatus);

public sealed record GetRegistrationsResponse(
    IReadOnlyList<RegistrationListItemResponse> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages);
