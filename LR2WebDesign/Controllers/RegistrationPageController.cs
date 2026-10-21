using LR2WebDesign.Controllers.APIRequestBodies;
using LR2WebDesign.Controllers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LR2WebDesign.Controllers
{
    [ApiController]
    [Route("registration")]
    public class RegistrationPageController : Controller
    {
        private IUserService _userService;

        public RegistrationPageController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> RegUser([FromBody] UserCreateRequestDTO UserCreateRequest,
                                                 [FromQuery] string? returnUrl,
                                                 IConfiguration config)
        {
            var tokenString = await _userService.CreateUser(UserCreateRequest, config);
            Response.Cookies.Append("accessToken", tokenString.Item1);
            return Ok(new { redirectUrl = returnUrl, accessToken = tokenString });
        }
    }   
}
