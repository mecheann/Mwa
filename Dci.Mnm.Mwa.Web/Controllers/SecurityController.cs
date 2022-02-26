using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dci.Mnm.Mwa.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SecurityController : Controller
    {
        IMediator mediator;

        public SecurityController(IMediator mediator)
        {
            this.mediator = mediator;
        }
    }
}
