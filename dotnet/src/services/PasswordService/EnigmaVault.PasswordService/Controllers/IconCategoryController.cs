using EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.CreateCommon;
using EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.CreatePersonal;
using EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.DeleteCommon;
using EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.DeletePersonal;
using EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.UpdateCommon;
using EnigmaVault.PasswordService.Application.Features.IconCategories.Commands.UpdatePersonal;
using EnigmaVault.PasswordService.Application.Features.IconCategories.Queries.GetAll;
using EnigmaVault.PasswordService.Application.Features.IconCategories.Queries.GetPersonal;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Requests.PasswordService;

namespace EnigmaVault.PasswordService.Controllers
{
    [ApiController]
    [Route("api/icon-categories")]
    public sealed class IconCategoryController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        /*--Create----------------------------------------------------------------------------------------*/
      
        [HttpPost("common")]
        public async Task<IActionResult> CreateCommon([FromBody] CreateIconCategoryCommonRequest request)
        {
            var result = await _mediator.Send(new CreateCommonIconCategoryCommand(request.Name));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        [HttpPost("personal")]
        public async Task<IActionResult> CreateCommon([FromBody] CreateIconCategoryPersonalRequest request)
        {
            var result = await _mediator.Send(new CreatePersonalCategoryCommand(request.Name, request.UserId));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Update----------------------------------------------------------------------------------------*/

        [HttpPatch("common")]
        public async Task<IActionResult> UpdateCommon([FromBody] UpdateCommonIconCategoryRequest request)
        {
            var result = await _mediator.Send(new UpdateCommonIconCategoryCommand(request.Id, request.Name));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        [HttpPatch("personal")]
        public async Task<IActionResult> UpdateCommon([FromBody] UpdatePersonalIconCategoryRequest request)
        {
            var result = await _mediator.Send(new UpdatePersonalIconCategoryCommand(request.Id, request.UserId, request.Name));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Delete----------------------------------------------------------------------------------------*/

        [HttpDelete("common/{id}")]
        public async Task<IActionResult> DeleteCommon([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new DeleteCommonIconCategoryCommand(id));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        [HttpDelete("personal/{userId}/{id}")]
        public async Task<IActionResult> DeleteCommon([FromRoute] Guid id, [FromRoute] Guid userId)
        {
            var result = await _mediator.Send(new DeletePersonalIconCategoryCommand(id, userId));

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(result.StringMessage));
        }

        /*--Get-------------------------------------------------------------------------------------------*/

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAll([FromRoute] Guid userId)
        {
            var result = await _mediator.Send(new GetAllIconCategoriesQuery(userId));

            return Ok(result);
        }

        [HttpGet("personal/{userId}")]
        public async Task<IActionResult> GetAllPersonal([FromRoute] Guid userId)
        {
            var result = await _mediator.Send(new GetAllPersonalIconCategories(userId));

            return Ok(result);
        }
    }
}