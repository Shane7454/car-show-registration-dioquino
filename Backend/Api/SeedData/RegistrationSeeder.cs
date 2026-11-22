namespace Api.SeedData;

using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public static class RegistrationSeeder
{
    private static readonly string[] ParticipantNames =
    [
        "Juan Dela Cruz", "Maria Santos", "Pedro Reyes", "Ana Garcia", "Carlos Mendoza",
        "Liza Cruz", "Miguel Ramos", "Sofia Bautista", "Andres Villanueva", "Bea Flores",
        "Paolo Navarro", "Nina Castillo", "Rafael Aquino", "Ella Mercado", "Marco Lim",
        "Isabel Torres", "Gabriel Salazar", "Chloe Rivera", "Daniel Santiago", "Mia Fernandez",
        "Luis Gonzales", "Ava Martinez", "Enzo Herrera", "Grace Valdez", "Noah Aguilar"
    ];

    public static async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken = default)
    {
        if (await dbContext.Registrations.AnyAsync(cancellationToken))
        {
            return;
        }

        var types = Enum.GetValues<RegistrationType>();
        var statuses = Enum.GetValues<RegistrationStatus>();
        var registrations = new List<Registration>(ParticipantNames.Length);

        for (var index = 0; index < ParticipantNames.Length; index++)
        {
            var registrationType = types[index % types.Length];
            var fee = GetFee(registrationType);
            var amountPaid = (index % 3) switch
            {
                0 => 0m,
                1 => fee / 2m,
                _ => fee
            };

            registrations.Add(new Registration
            {
                Id = Guid.Parse($"00000000-0000-0000-0000-{index + 1:D12}"),
                RegistrationNumber = $"CS27-{index + 1:D4}",
                RegistrationDate = DateTime.UtcNow.Date.AddDays(-index),
                ParticipantName = ParticipantNames[index],
                ContactNumber = $"0917{index + 1000000:D7}",
                Email = $"participant{index + 1}@example.com",
                Address = $"{index + 1} Rizal Street, Sorsogon City",
                CarMakeModel = GetCarMakeModel(index),
                PlateNumber = $"CS27-{index + 1:D4}",
                RegistrationType = registrationType,
                Status = statuses[index % statuses.Length],
                RegistrationFee = fee,
                AmountPaid = amountPaid
            });
        }

        await dbContext.Registrations.AddRangeAsync(registrations, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static decimal GetFee(RegistrationType registrationType) => registrationType switch
    {
        RegistrationType.Exhibitor => 1500m,
        RegistrationType.Competitor => 2500m,
        RegistrationType.VIP => 5000m,
        _ => throw new ArgumentOutOfRangeException(nameof(registrationType), registrationType, null)
    };

    private static string GetCarMakeModel(int index) => (index % 5) switch
    {
        0 => "Toyota Supra MK4",
        1 => "Honda Civic Type R",
        2 => "Nissan Skyline GT-R",
        3 => "Mazda RX-7",
        _ => "Mitsubishi Lancer Evo"
    };
}
