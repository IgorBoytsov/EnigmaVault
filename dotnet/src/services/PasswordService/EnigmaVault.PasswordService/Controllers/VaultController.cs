using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Requests.PasswordService;

namespace EnigmaVault.PasswordService.Controllers
{
    [ApiController]
    [Route("api/vault")]
    public class VaultController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        /*--Create----------------------------------------------------------------------------------------*/

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVaultItemRequest request)
        {
            var command = new CreateVaultItemCommand(
                     request.UserId,
                     request.PasswordType,
                     Convert.FromBase64String(request.EncryptedOverview),
                     Convert.FromBase64String(request.EncryptedDetails));

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok(result.Value);
        }
    }
}