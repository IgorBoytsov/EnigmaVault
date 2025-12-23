using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.AddToFavorites;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.Archive;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.Create;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.Delete;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.EmptyTrash;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.MoveToTrash;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.RemoveFromFavorites;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.RestoreAllFromTrash;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.RestoreFromTrash;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.UnArchive;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.Update;
using EnigmaVault.PasswordService.Application.Features.VaultItems.Queries.GetAll;
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

        /*--Update----------------------------------------------------------------------------------------*/

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateVaultItemRequest request)
        {
            var command = new UpdateVaultItemCommand(
                Guid.Parse(request.UserId),
                Guid.Parse(request.VaultItemId),
                Convert.FromBase64String(request.EncryptedOverview),
                Convert.FromBase64String(request.EncryptedDetails));

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok(result.Value);
        }

        [HttpPatch("add-favorites/{userId}/{vaultId}")]
        public async Task<IActionResult> AddToFavorites([FromRoute] Guid userId, [FromRoute] Guid vaultId)
        {
           var command = new AddToFavoritesVaultCommand(vaultId, userId);

            var result = await _mediator.Send(command);

           if (result.IsFailure)
               return BadRequest(result.StringMessage);
            
            return Ok();
        }

        [HttpPatch("remove-favorites/{userId}/{vaultId}")]
        public async Task<IActionResult> RemoveFromFavorites([FromRoute] Guid userId, [FromRoute] Guid vaultId)
        {
           var command = new RemoveFromFavoritesVaultCommand(vaultId, userId);

            var result = await _mediator.Send(command);

           if (result.IsFailure)
               return BadRequest(result.StringMessage);
            
            return Ok();
        }

        [HttpPatch("archive/{userId}/{vaultId}")]
        public async Task<IActionResult> Archive([FromRoute] Guid userId, [FromRoute] Guid vaultId)
        {
           var command = new ArchiveVaultCommand(vaultId, userId);

            var result = await _mediator.Send(command);

           if (result.IsFailure)
               return BadRequest(result.StringMessage);
            
            return Ok();
        }

        [HttpPatch("un-archive/{userId}/{vaultId}")]
        public async Task<IActionResult> UnArchive([FromRoute] Guid userId, [FromRoute] Guid vaultId)
        {
           var command = new UnArchiveVaultCommand(vaultId, userId);

            var result = await _mediator.Send(command);

           if (result.IsFailure)
               return BadRequest(result.StringMessage);
            
            return Ok();
        }

        /*--Delete----------------------------------------------------------------------------------------*/

        [HttpDelete("{userId}/{vaultId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid userId, Guid vaultId)
        {
            var command = new DeleteVaultItemCommand(userId, vaultId);

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok();
        }

        [HttpPatch("empty-trash/{userId}")]
        public async Task<IActionResult> EmptyTrash([FromRoute] Guid userId)
        {
            var command = new EmptyVaultsTrashCommand(userId);

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok();
        }

        [HttpPatch("move-to-trash/{userId}/{vaultId}")]
        public async Task<IActionResult> MoveToTrash([FromRoute] Guid userId, Guid vaultId)
        {
            var command = new MoveVaultToTrashCommand(userId, vaultId);

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok(result.Value);
        }

        [HttpPatch("restore-from-trash/{userId}/{vaultId}")]
        public async Task<IActionResult> RestoreFronmTrash([FromRoute] Guid userId, Guid vaultId)
        {
            var command = new RestoreVaultFromTrashCommand(userId, vaultId);

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok();
        }

        [HttpPatch("restore-all-from-trash/{userId}")]
        public async Task<IActionResult> RestoreAllFronmTrash([FromRoute] Guid userId)
        {
            var command = new RestoreAllVaultsFromTrashCommand(userId);

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.StringMessage);

            return Ok();
        }

        /*--Get-------------------------------------------------------------------------------------------*/

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAll([FromRoute] Guid userId)
        {
            var result = await _mediator.Send(new GetAllVaultsQuery(userId));

            return Ok(result.Value);
        }
    }
}