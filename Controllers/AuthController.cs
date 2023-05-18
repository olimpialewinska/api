using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly InMemoryDbContext _dbContext;

        public AuthController(IConfiguration configuration, InMemoryDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;

        }

        // POST api/auth/register
        [HttpPost("register")]
        public IActionResult Register(RegisterModel registerModel)
        {
            try
            {
                var existingUser = _dbContext.Users.FirstOrDefault(u => u.Email == registerModel.Email);
                if (existingUser != null)
                    return BadRequest("Użytkownik o podanym adresie email już istnieje.");

                var newUser = new UserModel();
                newUser.Id = new Guid();
                newUser.Password = registerModel.Password;
                newUser.Email = registerModel.Email;
                newUser.Token = "";
                newUser.Tasks = new List<TaskModel> { new TaskModel { Id = new Guid(), IsCompleted = false, Description = "d" } };

                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();
                return Ok(newUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił wyjątek: {ex.Message}");
                return StatusCode(500);
            }
        }

        // POST api/auth/login
        [HttpPost("login")]
        public IActionResult Login(RegisterModel loginModel)
        {
            try
            {
                var existingUser = _dbContext.Users.FirstOrDefault(u => u.Email == loginModel.Email && u.Password == loginModel.Password);
                if (existingUser == null)
                    return Unauthorized("Nieprawidłowy adres email lub hasło.");

                var token = GenerateToken(existingUser);

                existingUser.Token = token;

                return Ok(existingUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił wyjątek: {ex.Message}");
                return StatusCode(500);
            }
        }

        private string GenerateToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),

            };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}


