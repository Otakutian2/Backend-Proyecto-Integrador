using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proyecto_backend.Interfaces;
using proyecto_backend.Schemas;
using proyecto_backend.Utils;

namespace proyecto_backend.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly IUser _userService;

        public UserController(IUser userService)
        {
            _userService = userService;
        }

        [HttpPost("verify-email")]
        public async Task<ActionResult<bool>> VerifyEmail([FromBody] VerifyEmail verifyEmail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var email = verifyEmail.Email.ToLower();

            var user = await _userService.GetByEmail(email);

            if (user == null)
            {
                return NotFound("Usuario no encontrado");
            }

            return Ok(true);
        }

        [HttpPost("verify-code")]
        public async Task<ActionResult<bool>> VerifyCode([FromBody] VerifyCode verifyCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var email = verifyCode.Email.ToLower();

            var user = await _userService.GetByEmail(email);

            if (user == null)
            {
                return NotFound("Usuario no encontrado");
            }

            if (user.Code == 0)
            {
                return Conflict("No se ha enviado una solicitud de cambio");
            }

            var result = _userService.VerifyCode(user, verifyCode.Code);
            return Ok(result);
        }

        [HttpPost("send-code")]
        public async Task<ActionResult<bool>> SendCode([FromBody] VerifyEmail verifyEmail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var email = verifyEmail.Email.ToLower();

            var user = await _userService.GetByEmail(email);

            if (user == null)
            {
                return NotFound("Usuario no encontrado");
            }

            var resutl = await _userService.SendCode(user);

            return Ok(resutl);
        }

        [HttpPost("change-password")]
        public async Task<ActionResult<bool>> ChangePassword([FromBody] ChangePassword changePassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var email = changePassword.Email.ToLower();

            var user = await _userService.GetByEmail(email);

            if (user == null)
            {
                return NotFound("Usuario no encontrado");
            }

            var codeIsCorrect = _userService.VerifyCode(user, changePassword.Code);

            if (!codeIsCorrect)
            {
                return Conflict("El código proporcionado es incorrecto");
            }

            user.Password = SecurityUtils.HashPassword(changePassword.NewPassword);

            var result = await _userService.ChangePassword(user, changePassword.NewPassword);

            return result;
        }
    }
}
