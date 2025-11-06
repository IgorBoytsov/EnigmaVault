using EnigmaVault.PasswordService.Application.Features.Folders.Commands.CreateRoot;
using EnigmaVault.PasswordService.Application.Features.Folders.Commands.CreateSubFolder;
using EnigmaVault.PasswordService.Application.Features.Folders.Commands.Delete;
using EnigmaVault.PasswordService.Application.Features.Folders.Commands.Update;
using EnigmaVault.PasswordService.Application.Features.Folders.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Requests.PasswordService;

namespace EnigmaVault.PasswordService.Controllers
{
    [ApiController]
    [Route("api/folders")]
    public sealed class FolderController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        /*--Create----------------------------------------------------------------------------------------*/

        [HttpPost("root")]
        public async Task<IActionResult> CreateRoot([FromBody] CreateFolderRootRequest request)
        {
            var result = await _mediator.Send(new CreateRootFolderCommand(Guid.Parse(request.UserId), request.Name, request.Color));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        [HttpPost("sub")]
        public async Task<IActionResult> CreateSubFolder([FromBody] CreateSubFolderRequest request)
        {
            var result = await _mediator.Send(new CreateSubFolderCommand(Guid.Parse(request.UserId), Guid.Parse(request.ParentFolderId), request.Name, request.Color));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Update----------------------------------------------------------------------------------------*/

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateFolderRequest request)
        {
            var result = await _mediator.Send(new UpdateFolderCommand(Guid.Parse(request.Id), Guid.Parse(request.UserId), request.ParentFolderId != null ? Guid.Parse(request.ParentFolderId) : null, request.Name));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Delete----------------------------------------------------------------------------------------*/

        [HttpDelete("{id}/{userId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, [FromRoute] Guid userId)
        {
            var result = await _mediator.Send(new DeleteFolderCommand(id, userId));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Get-------------------------------------------------------------------------------------------*/

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAll([FromRoute] Guid userId)
        {
            var result = await _mediator.Send(new GetAllFoldersQuery(userId));

            return Ok(result);
        }
    }
}