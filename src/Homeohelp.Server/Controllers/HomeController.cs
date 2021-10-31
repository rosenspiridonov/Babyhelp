namespace Homeohelp.Server.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : ApiController
    {
        [Authorize]
        public IActionResult Get()
        {
            return Ok("It works!");
        }
    }
}
