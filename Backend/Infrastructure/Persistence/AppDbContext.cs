namespace Infrastructure.Persistence;

using Application.Abstractions.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Registration> Registrations => Set<Registration>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ConfigureRegistration(modelBuilder.Entity<Registration>());
    }

    private static void ConfigureRegistration(EntityTypeBuilder<Registration> registration)
    {
        registration.HasKey(x => x.Id);

        registration.Property(x => x.RegistrationNumber)
            .IsRequired()
            .HasMaxLength(20);
        registration.HasIndex(x => x.RegistrationNumber)
            .IsUnique();

        registration.Property(x => x.RegistrationDate)
            .IsRequired();
        registration.Property(x => x.ParticipantName)
            .IsRequired()
            .HasMaxLength(150);
        registration.Property(x => x.ContactNumber)
            .IsRequired()
            .HasMaxLength(11);
        registration.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(200);
        registration.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(300);
        registration.Property(x => x.CarMakeModel)
            .IsRequired()
            .HasMaxLength(100);
        registration.Property(x => x.PlateNumber)
            .IsRequired()
            .HasMaxLength(15);
        registration.HasIndex(x => x.PlateNumber)
            .IsUnique();

        registration.Property(x => x.RegistrationType)
            .IsRequired();
        registration.Property(x => x.Status)
            .IsRequired();
        registration.Property(x => x.RegistrationFee)
            .IsRequired()
            .HasPrecision(10, 2);
        registration.Property(x => x.AmountPaid)
            .IsRequired()
            .HasPrecision(10, 2);
        registration.Ignore(x => x.PaymentStatus);
    }
}
