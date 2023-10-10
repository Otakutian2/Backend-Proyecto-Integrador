using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project_backend.Interfaces;
using project_backend.Schemas;
using project_backend.Utils;

namespace project_backend.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    public class AuthenticationController : Controller
    {
        private readonly IUser _userService;
        private readonly IAuth _authService;

        public AuthenticationController(IUser userService, IAuth authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest user)
        {
            // Validar datos de entrada
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userFromBD = await _userService.GetByEmail(user.Email);

            if (userFromBD == null || !SecurityUtils.CheckPassword(userFromBD.Password, user.Password))
            {
                return Unauthorized(new
                {
                    message = "Correo electrónico o contraseña incorrecta"
                });
            }

            var token = _authService.GenerateJWTToken(userFromBD);

            return Ok(new AuthResponse
            {
                AccessToken = token
            });
        }

        [HttpGet]
        [Authorize]
        [Route("GetCurrentUser")]
        public async Task<ActionResult<EmployeeGet>> GetCurrentUser()
        {
            return Ok((await _authService.GetCurrentUser()).Adapt<EmployeeGet>());
        }
    }
}
