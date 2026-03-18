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
	[Route("api/[controller]")] // istekler "api/auth"  yönlenecek
	public class AuthController : ControllerBase
	{
		private readonly IUserRepository _userRepository;

		public AuthController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		//  api/auth/register (Kayýt Olma Uç Noktasý)
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] User newUser)
		{
			
			var existingUser = await _userRepository.GetUserByEmailAsync(newUser.Email);
			if (existingUser != null)
			{
				// RMM-2 409
				return Conflict(new { Message = "Bu email adresi zaten sistemde kayýtlý." });
			}

			// Öðrenci atama
			if (string.IsNullOrEmpty(newUser.Role))
			{
				newUser.Role = "Student";
			}

			// MongoDb kayýt
			await _userRepository.CreateUserAsync(newUser);

			// RMM-2 201 
			return StatusCode(201, new { Message = "Kullanýcý baþarýyla oluþturuldu.", UserId = newUser.Id });
		}

		// POST: api/auth/login (Giriþ Yapma ve Token Üretme Uç Noktasý)
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			
			var user = await _userRepository.GetUserByEmailAsync(request.Email);

			// 2. Kullanýcý yoksa veya þifre yanlýþsa 401 Unauthorized dön
			if (user == null || user.PasswordHash != request.Password)
			{
				return Unauthorized(new { Message = "Email veya þifre hatalý." });
			}

			//  JWT ÜRETÝMÝ
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
				Expires = DateTime.UtcNow.AddHours(2), // Token 2 saat geçerli olacak
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			var jwtString = tokenHandler.WriteToken(token);

			// 4. Baþarýlý giriþ ve Token'ý kullanýcýya teslim etme
			return Ok(new { Message = "Giriþ baþarýlý!", Token = jwtString });
		}
	}

	// Login JSON
	public class LoginRequest
	{
		public string Email { get; set; }
		public string Password { get; set; }
	}
}