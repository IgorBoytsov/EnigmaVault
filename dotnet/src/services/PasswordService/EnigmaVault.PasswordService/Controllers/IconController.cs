using EnigmaVault.PasswordService.Application.Features.Icons.Commands.CreateCommon;
using EnigmaVault.PasswordService.Application.Features.Icons.Commands.CreatePersonal;
using EnigmaVault.PasswordService.Application.Features.Icons.Commands.DeleteCommon;
using EnigmaVault.PasswordService.Application.Features.Icons.Commands.DeletePersonal;
using EnigmaVault.PasswordService.Application.Features.Icons.Commands.UpdateCommon;
using EnigmaVault.PasswordService.Application.Features.Icons.Commands.UpdatePersonal;
using EnigmaVault.PasswordService.Application.Features.Icons.Queries.GetAll;
using EnigmaVault.PasswordService.Application.Features.Icons.Queries.GetPersonal;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Requests.PasswordService;

namespace EnigmaVault.PasswordService.Controllers
{
    [ApiController]
    [Route("api/icons")]
    public sealed class IconController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        /*--Create----------------------------------------------------------------------------------------*/

        [HttpPost("common")]
        public async Task<IActionResult> CreateCommon([FromBody] CreateIconCommonRequest request)
        {
            var result = await _mediator.Send(new CreateCommonIconCommand(request.SvgCode, request.Name, Guid.Parse(request.IconCategoryId)));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        [HttpPost("personal")]
        public async Task<IActionResult> CreatePersonal([FromBody] CreateIconPersonalRequest request)
        {
            var result = await _mediator.Send(new CreatePersonalIconCommand(Guid.Parse(request.UserId), request.SvgCode,request.Name, Guid.Parse(request.IconCategoryId)));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(result.Value),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Update----------------------------------------------------------------------------------------*/

        [HttpPatch("common")]
        public async Task<IActionResult> UpdateCommon([FromBody] UpdateCommonIconRequest request)
        {
            var result = await _mediator.Send(new UpdateCommonIconCommand(Guid.Parse(request.Id), request.Name, request.SvgCode, Guid.Parse(request.IconCategoryId)));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        [HttpPatch("personal")]
        public async Task<IActionResult> UpdatePersonal([FromBody] UpdatePersonalIconRequest request)
        {
            var result = await _mediator.Send(new UpdatePersonalIconCommand(Guid.Parse(request.Id), Guid.Parse(request.UserId), request.Name, request.SvgCode, Guid.Parse(request.IconCategoryId)));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Delete----------------------------------------------------------------------------------------*/

        [HttpDelete("common/{id}")]
        public async Task<IActionResult> DeleteCommon([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new DeleteCommonIconCommand(id));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        [HttpDelete("personal/{userId}/{id}")]
        public async Task<IActionResult> DeletePersonal([FromRoute] Guid id, [FromRoute] Guid userId)
        {
            var result = await _mediator.Send(new DeletePersonalIconCommand(id, userId));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Get-------------------------------------------------------------------------------------------*/

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAll([FromRoute] Guid userId)
        {
            var result = await _mediator.Send(new GetAllIconQuery(userId));

            return Ok(result);
        }

        [HttpGet("personal/{userId}")]
        public async Task<IActionResult> GetAllPersonal([FromRoute] Guid userId)
        {
            var result = await _mediator.Send(new GetAllPersonalIconQuery(userId));

            return Ok(result);
        }
    }
}