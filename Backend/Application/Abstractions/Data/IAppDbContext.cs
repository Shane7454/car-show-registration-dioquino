namespace Application.Abstractions.Data;

using Microsoft.EntityFrameworkCore;
using Domain.Entities;

public interface IAppDbContext
{
    DbSet<Registration> Registrations { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
