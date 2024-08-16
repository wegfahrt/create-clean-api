using Application.Interfaces;
using Domain.Users;
using ErrorOr;
using MediatR;

namespace Application.Users.Queries;

public class GetUserQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserQuery, ErrorOr<User>>
{
    private readonly IUserRepository _userRepository = userRepository;

    public Task<ErrorOr<User>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return _userRepository.GetByIdAsync(request.Id);
    }
}
