//using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace WebCinema.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly WebCinemaDBContext _dbContext;

        public AuthController(IConfiguration config, WebCinemaDBContext dbContext)
        {
            _config = config;
            _dbContext = dbContext;
        }

        [HttpPost]
        public ActionResult Login([FromBody] LoginModel model)
        {
            var user = _dbContext.Users
                .FirstOrDefault(u => u.Username == model.Username);

            if (user != null && user.Password == model.Password)
            {
                var token = GenerateJwtToken(user.Username);
                return Ok(new { token }); // OVO JE KLJUČNO!
            }

            return Unauthorized();
        }

        private string GenerateJwtToken(string username)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var user = _dbContext.Users
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.Username == username);
            if (user == null)
                return null;
            var role = user.Roles?.Name ?? "User";  // Default to "User" if role is null
            var claims = new[]
            {
                 new Claim(ClaimTypes.Name, username),
                 new Claim(ClaimTypes.Role, role),
                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

    }
}
