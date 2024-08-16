using ErrorOr;
using Microsoft.EntityFrameworkCore;

using Application.Interfaces;
using Domain.Users;
using Infrastructure.Common.Persistence;

namespace Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext _context = context;

    public async Task<ErrorOr<User>> GetByIdAsync(int id)
    {
        var user = await _context.Users
                .FindAsync(id);

        if (user == null)
            return Error.NotFound("User not found");

        return user;
    }
}


