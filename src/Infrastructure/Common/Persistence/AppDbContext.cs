using Microsoft.EntityFrameworkCore;

using Domain.Users;
using Application.Interfaces;

namespace Infrastructure.Common.Persistence;

//TODO: Should probably be defined under Infrastructure/Common/Persistence/AppDbContext.cs
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IUnitOfWork
{
    public async Task CommitChangesAsync()
    {
        await SaveChangesAsync();
    }

    public DbSet<User> Users { get; set; }
}
