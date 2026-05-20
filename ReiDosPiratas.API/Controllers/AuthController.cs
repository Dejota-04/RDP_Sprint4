using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReiDosPiratas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            // Tradeoff de Segurança: Para a agilidade da Sprint 4, estamos usando credenciais fixas.
            // Em um cenário real (especialmente com dados sensíveis ou transações), validaríamos
            // isso contra o banco de dados Oracle usando senhas com hash e salt (ex: ASP.NET Core Identity).
            if (login.Username == "admin" && login.Password == "admin123")
            {
                var token = GerarTokenJwt(login.Username);
                return Ok(new { token });
            }

            return Unauthorized(new { mensagem = "Usuário ou senha inválidos." });
        }

        private string GerarTokenJwt(string username)
        {
            // Puxa a chave do appsettings ou usa o fallback que configuramos no Program.cs
            var jwtKey = _configuration["Jwt:Key"] ?? "UmaChaveSuperSecretaParaSuaAPIFuncionarNaSprint4!";
            var key = Encoding.ASCII.GetBytes(jwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin")
                }),
                Expires = DateTime.UtcNow.AddHours(2), // Token expira em 2 horas
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }

    // DTO auxiliar apenas para receber os dados de login
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}