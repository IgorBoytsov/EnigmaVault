using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EnigmaVault.PasswordService.Controllers
{
    [ApiController]
    [Route("api/vault")]
    public class VaultController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;
    }
}