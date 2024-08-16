using Application.Interfaces;
using Domain.Users;
using ErrorOr;
using MediatR;

namespace Application.Users.Queries;

public record GetUserQuery(int Id) : IRequest<ErrorOr<User>>;
