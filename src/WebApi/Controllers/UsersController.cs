using MediatR;
using Microsoft.AspNetCore.Mvc;

using Application.Users.Queries;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var query = new GetUserQuery(id);
        var result = await _mediator.Send(query);

        return result.Match(
            user => Ok(user),
            errors => Problem(errors.ToString())
        );
    }
}

