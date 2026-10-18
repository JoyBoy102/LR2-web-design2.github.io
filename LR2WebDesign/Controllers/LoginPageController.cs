using LR2WebDesign.Controllers.APIRequestBodies;
using LR2WebDesign.Controllers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LR2WebDesign.Controllers
{
    [ApiController]
    [Route("authorization")]
    public class LoginPageController : Controller
    {
        private IUserService _userService;

        public LoginPageController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IResult> LoginUser([FromBody] UserLoginRequestDTO userLoginRequest, IConfiguration config)
        {
            var tokenString = await _userService.LoginUser(userLoginRequest, config);
            Response.Cookies.Append("accessToken", tokenString);
            return Results.Ok(new { accessToken = tokenString, redirectUrl = "/" });
        }
    }
}
