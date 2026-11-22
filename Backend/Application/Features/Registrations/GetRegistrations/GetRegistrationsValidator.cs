namespace Application.Features.Registrations.GetRegistrations;

using Domain.Enums;
using FluentValidation;

public sealed class GetRegistrationsValidator : AbstractValidator<GetRegistrationsQuery>
{
    public GetRegistrationsValidator()
    {
        RuleFor(x => x.Type)
            .Must(IsValidRegistrationType)
            .WithMessage("Type must be Exhibitor, Competitor, or VIP.");

        RuleFor(x => x.Status)
            .Must(IsValidRegistrationStatus)
            .WithMessage("Status must be Pending, Confirmed, CheckedIn, or Cancelled.");

        RuleFor(x => x.ParticipantName)
            .MaximumLength(150);

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 50)
            .WithMessage("PageSize must be between 1 and 50.");
    }

    private static bool IsValidRegistrationType(string? value) =>
        string.IsNullOrWhiteSpace(value) ||
        Enum.TryParse<RegistrationType>(value, ignoreCase: true, out var type) && Enum.IsDefined(type);

    private static bool IsValidRegistrationStatus(string? value) =>
        string.IsNullOrWhiteSpace(value) ||
        Enum.TryParse<RegistrationStatus>(value, ignoreCase: true, out var status) && Enum.IsDefined(status);
}
