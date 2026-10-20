using LR2WebDesign.Controllers.APIRequestBodies;
using LR2WebDesign.Controllers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LR2WebDesign.Controllers
{
    [ApiController]
    [Route("admin")]
    public class AdminPageController : Controller
    {
        private IUserService _userService;

        public AdminPageController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("createUser")]
        public async Task<IActionResult> RegUser([FromBody] UserCreateRequestDTO UserCreateRequest, IConfiguration config)
        {
            var response = await _userService.CreateUser(UserCreateRequest, config);
            return Ok(new { userId = response.Item2 });
        }

        [HttpPost("deleteUser")]
        public async Task<IActionResult> DeleteUser(int userId, IConfiguration config)
        {
            await _userService.DeleteUser(userId);
            return Ok();
        }
    }
}
