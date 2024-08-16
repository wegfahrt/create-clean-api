using Domain.Users;
using Infrastructure.Common.Persistence;

namespace Tests.ExampleUtil;

public class DbInitializer
{
    private readonly AppDbContext _context;

    public DbInitializer(AppDbContext context)
    {
        _context = context;
    }

    public void Initialize()
    {
        _context.Database.EnsureCreated();

        if (!_context.Users.Any())
        {
            var users = new[]
            {
                User.Create(1, "John Doe", "john@example.com").Value,
                User.Create(2, "Jane Smith", "jane@example.com").Value,
                User.Create(3, "Bob Johnson", "bob@example.com").Value
            };

            _context.Users.AddRange(users);
            _context.SaveChanges();
        }
    }
}
