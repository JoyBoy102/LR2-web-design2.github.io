using LR2WebDesign.Controllers.APIRequestBodies;
using LR2WebDesign.Controllers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LR2WebDesign.Controllers
{
    [Route("registration")]
    public class RegistrationPageController : Controller
    {
        private IUserService _userService;

        public RegistrationPageController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> RegUser([FromBody] UserRequestBody userRequestBody)
        {
            _userService.CreateUser(userRequestBody);
            return Ok();
        }
    }   
}
