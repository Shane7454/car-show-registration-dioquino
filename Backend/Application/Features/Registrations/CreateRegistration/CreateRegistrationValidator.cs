namespace Application.Features.Registrations.CreateRegistration;

using Domain.Enums;
using FluentValidation;

public sealed class CreateRegistrationValidator : AbstractValidator<CreateRegistrationCommand>
{
    public CreateRegistrationValidator()
    {
        RuleFor(x => x.ParticipantName)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.ContactNumber)
            .NotEmpty()
            .Matches("^09[0-9]{9}$")
            .WithMessage("ContactNumber must be an 11-digit Philippine mobile number starting with 09.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(200);

        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(300);

        RuleFor(x => x.CarMakeModel)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.PlateNumber)
            .NotEmpty()
            .MaximumLength(15);

        RuleFor(x => x.RegistrationType)
            .Must(type => Enum.IsDefined(type))
            .WithMessage("RegistrationType must be Exhibitor, Competitor, or VIP.");

        RuleFor(x => x.AmountPaid)
            .GreaterThanOrEqualTo(0)
            .Must((command, amount) => amount <= GetFee(command.RegistrationType))
            .When(x => Enum.IsDefined(x.RegistrationType))
            .WithMessage("AmountPaid must be between zero and the fee for the selected registration type.");
    }

    private static decimal GetFee(RegistrationType type) => type switch
    {
        RegistrationType.Exhibitor => 1500m,
        RegistrationType.Competitor => 2500m,
        RegistrationType.VIP => 5000m,
        _ => 0m
    };
}
