using CarbCounter.Application.Account.Commands.Authenticate;
using CarbCounter.Application.Account.Commands.Create;
using CarbCounter.Application.Account.Commands.HardDelete;
using CarbCounter.Application.Account.Commands.Recover;
using CarbCounter.Application.Account.Commands.SoftDelete;
using CarbCounter.Application.Account.Dto;
using CarbCounter.Application.Common.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CarbCounter.WebApi.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    [HttpPost("/register")]
    public async Task<ActionResult<string>> Register(
        [FromBody] CreateAccountCommand command,
        [FromServices] CreateAccountCommandHandler handler,
        CancellationToken cancellationToken)
    {
        AppResponse<string> appResponse = await handler.Handle(command, cancellationToken);

        return appResponse.ToActionResult();
    }
    
    [HttpPost("/authenticate")]
    public async Task<ActionResult<AuthenticateAccountDto>> Login(
        [FromBody] AuthenticateAccountCommand command,
        [FromServices] AuthenticateAccountCommandHandler handler,
        CancellationToken cancellationToken)
    {
        AppResponse<AuthenticateAccountDto> appResponse = await handler.Handle(command, cancellationToken);

        return appResponse.ToActionResult();
    }

    [HttpDelete("{id}/soft")]
    public async Task<ActionResult> SoftDelete(
        [FromRoute] string id,
        [FromServices] SoftDeleteAccountCommandHandler handler,
        CancellationToken cancellationToken
        )
    {
        AppResponse appResponse = await handler.Handle(new()
        {
            Id = id
        }, cancellationToken);

        return appResponse.ToActionResult();
    }
    
    [HttpDelete("{id}/hard")]
    public async Task<ActionResult> HardDelete(
        [FromRoute] string id,
        [FromServices] HardDeleteAccountCommandHandler handler,
        CancellationToken cancellationToken
    )
    {
        AppResponse appResponse = await handler.Handle(new()
        {
            Id = id
        }, cancellationToken);

        return appResponse.ToActionResult();
    }
    
    [HttpPost("{id}/recover")]
    public async Task<ActionResult> Recover(
        [FromRoute] string id,
        [FromServices] RecoverAccountCommandHandler handler,
        CancellationToken cancellationToken
    )
    {
        AppResponse appResponse = await handler.Handle(new()
        {
            Id = id
        }, cancellationToken);

        return appResponse.ToActionResult();
    }
}