using iso_management_system.Services;
using Microsoft.AspNetCore.Mvc;

namespace iso_management_system.Controllers
{
    
    
    [ApiController]
    [Route("api/[controller]")] // [controller] is a token that ASP.NET Core automatically replaces with the controller’s class name minus the "Controller" suffix
    public class GeneralController : ControllerBase
    {
        private readonly GeneralService _generalService;

        public GeneralController(GeneralService generalService)
        {
            _generalService = generalService;
        }

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = _generalService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            var roles = _generalService.GetAllRoles();
            return Ok(roles);
        }
    }
}