using Microsoft.IdentityModel.Tokens;
using project_backend.Interfaces;
using project_backend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace project_backend.Services
{
    public class AuthService : IAuth
    {
        private readonly IEmployee _employeeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public AuthService(IHttpContextAccessor httpContextAccessor, IEmployee employeeService, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _employeeService = employeeService;
            _configuration = configuration;
        }

        public string GenerateJWTToken(User user)
        {
            // Obtenemos nuestra secretKey en formato de bytes
            var secretKeyBytes = Encoding.UTF8.GetBytes(_configuration["JWTSettings:SecretKey"]);

            var employee = user.Employee;

            // Declaramos una lista de claims las cuáles serán información de nuestro token
            var claimsList = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, employee.Id.ToString()),
                new Claim(ClaimTypes.Role, employee.Role.Name)
            };

            // Se añade algunas configuraciones al token, como la expiración y los claims
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration["JWTSettings:Issuer"],
                audience: _configuration["JWTSettings:Audience"],
                claims: claimsList,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(secretKeyBytes),
                    SecurityAlgorithms.HmacSha256)
            );

            // Escribimos el token en formato de string
            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return token;
        }

        public async Task<Employee> GetCurrentUser()
        {
            var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var id = int.Parse(identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value);

            return await _employeeService.GetById(id);
        }

        public int GetCurrentUserId()
        {
            var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var id = int.Parse(identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value);
            return id;
        }

        public string GetCurrentUserRole()
        {
            var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var role = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            return role;
        }
    }
}
