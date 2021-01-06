using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RLTestTask.Services;

namespace RLTestTask.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]AuthRequestBody requestBody)
        {
            AuthResponse response = await authService.Authenticate(requestBody.Login, requestBody.Password);
            if (response == null)
            {
                return BadRequest(new { errorMessage = "Invalid username/password" });
            }
            return Json(response);
        }
    }

    public class AuthRequestBody
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}