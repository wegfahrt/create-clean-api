using ErrorOr;

using Domain.Users;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<ErrorOr<User>> GetByIdAsync(int id);
}

