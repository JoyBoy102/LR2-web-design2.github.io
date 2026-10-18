using LR2WebDesign.Controllers.APIRequestBodies;
using LR2WebDesign.Controllers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LR2WebDesign.Controllers
{
    [ApiController]
    public class MainPageController : Controller
    {
        private readonly IWebHostEnvironment _env;
        public MainPageController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet("registration")]
        public IActionResult GetRegistrationPage()
        {
            var path = Path.Combine(_env.WebRootPath, "registration.html");
            return PhysicalFile(path, "text/html");
        }

        [HttpGet("ai-constructor")]
        [Authorize]
        public IActionResult GetAiConstructorPage()
        {
            var path = Path.Combine(_env.WebRootPath, "ai-constructor.html");
            return PhysicalFile(path, "text/html");
        }

        [HttpGet("authorization")]
        public IActionResult GetLoginPage()
        {
            var path = Path.Combine(_env.WebRootPath, "login.html");
            return PhysicalFile(path, "text/html");
        }

        [HttpGet("/")]
        public IActionResult GetMainPage()
        {
            var path = Path.Combine(_env.WebRootPath, "index.html");
            return PhysicalFile(path, "text/html");
        }
    }
}
