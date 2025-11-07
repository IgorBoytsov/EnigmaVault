using EnigmaVault.PasswordService.Application.Features.Tags.Commands.Create;
using EnigmaVault.PasswordService.Application.Features.Tags.Commands.Delete;
using EnigmaVault.PasswordService.Application.Features.Tags.Commands.Update;
using EnigmaVault.PasswordService.Application.Features.Tags.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Requests.PasswordService;

namespace EnigmaVault.PasswordService.Controllers
{
    [ApiController]
    [Route("api/tags")]
    public class TagController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        /*--Create----------------------------------------------------------------------------------------*/

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTagRequest request)
        {
            var result = await _mediator.Send(new CreateTagCommand(Guid.Parse(request.UserId), request.Name));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Update----------------------------------------------------------------------------------------*/

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateTagRequest request)
        {
            var result = await _mediator.Send(new UpdateTagCommand(Guid.Parse(request.Id), request.Name));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Delete----------------------------------------------------------------------------------------*/

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new DeleteTagCommand(id));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }
        /*--Get-------------------------------------------------------------------------------------------*/

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAll([FromRoute] Guid userId)
        {
            var result = await _mediator.Send(new GetAllTagsQuery(userId));

            return Ok(result);
        }
    }
}