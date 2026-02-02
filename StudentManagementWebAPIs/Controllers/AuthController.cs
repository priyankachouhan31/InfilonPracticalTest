using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentManagementWebAPIs.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentManagementWebAPIs.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        internal static string token;
            private string username = "Admin";
        private string password = "Admin123";
        private readonly IConfiguration configuration;

        public AuthController(IConfiguration config)
        {
            configuration = config;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> GetAccessToken([FromBody] LoginModel model)
        {
            try
            {
                if (model.Username == username && model.Password == password)
                {
                    var token = GenerateToken(model.Username, configuration);
                    return StatusCode(201, new { token });
                }
                return StatusCode(401, "Unauthorized Access!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "exception " + ex.Message);
            }
        }

        private string GenerateToken(string username, IConfiguration config)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            byte[] key = Encoding.ASCII.GetBytes(config["Jwt:Key"]);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, username)
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}
