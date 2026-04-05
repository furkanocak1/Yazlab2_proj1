using AuthService.Models;
using AuthService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IUserRepository _userRepository;

		public AuthController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		// api/auth/register — Email/PasswordHash veya (test) Username/Password
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterBody body)
		{
			var email = body.Email;
			if (string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(body.Username))
				email = body.Username.Contains('@', StringComparison.Ordinal)
					? body.Username
					: $"{body.Username}@gateway.test";

			var passwordStored = !string.IsNullOrEmpty(body.PasswordHash) ? body.PasswordHash : body.Password;
			var studentId = !string.IsNullOrWhiteSpace(body.StudentId) ? body.StudentId! : (body.Username ?? "unknown");

			if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(passwordStored))
				return BadRequest(new { Message = "Email veya sifre eksik." });

			var newUser = new User
			{
				Email = email,
				PasswordHash = passwordStored,
				StudentId = studentId,
				Role = string.IsNullOrEmpty(body.Role) ? "Student" : body.Role!
			};

			var existingUser = await _userRepository.GetUserByEmailAsync(newUser.Email);
			if (existingUser != null)
				return Conflict(new { Message = "Bu email adresi zaten sistemde kayitli." });

			await _userRepository.CreateUserAsync(newUser);

			return StatusCode(201, new { Message = "Kullanici basariyla olusturuldu.", UserId = newUser.Id });
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			var user = await _userRepository.GetUserByEmailAsync(request.Email);

			if (user == null || user.PasswordHash != request.Password)
			{
				return Unauthorized(new { Message = "Email veya sifre hatali." });
			}

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes("YazlabBiletlemeSistemiCokGizliAnahtar12345!!");

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.NameIdentifier, user.Id),
					new Claim(ClaimTypes.Email, user.Email),
					new Claim(ClaimTypes.Role, user.Role)
				}),
				Expires = DateTime.UtcNow.AddHours(2),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			var jwtString = tokenHandler.WriteToken(token);

			return Ok(new { Message = "Giris basarili!", Token = jwtString });
		}
	}

	public class RegisterBody
	{
		public string? Username { get; set; }
		public string? Password { get; set; }
		public string? Email { get; set; }
		public string? PasswordHash { get; set; }
		public string? StudentId { get; set; }
		public string? Role { get; set; }
	}

	public class LoginRequest
	{
		public string Email { get; set; }
		public string Password { get; set; }
	}
}
