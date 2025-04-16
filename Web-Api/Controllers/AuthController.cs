using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Api.Data;
using Web_Api.Model;
using Web_Api.Services; // Подключение PasswordHelper
using System.Security.Cryptography;
using System.Text;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TeamProfitDBContext _context;

        public AuthController(TeamProfitDBContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _context.Users.AnyAsync(u => u.Email == registerModel.Email))
                return BadRequest("Пользователь с таким email уже существует.");

            if (registerModel.Password.Length < 6)
                return BadRequest("Пароль слишком простой. Минимум 6 символов.");

            // 🔐 Проверка на сложность пароля
            var passwordErrors = PasswordStrengthService.GetWeaknesses(registerModel.Password);
            if (passwordErrors.Any())
            {
                return BadRequest(new
                {
                    Message = "Пароль недостаточно надёжен.",
                    Errors = passwordErrors
                });
            }

            var salt = PasswordHelper.GenerateSalt();
            var hash = PasswordHelper.ComputeHash(registerModel.Password, salt);

            var user = new User
            {
                Login = registerModel.Login,
                Email = registerModel.Email,
                PasswordHash = hash,
                Salt = salt,
                PriceWork = 10,
                IsActive = true,
                DateRegistration = DateTime.UtcNow,
                IdTeam = 0
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Регистрация прошла успешно.");
        }


        // Авторизация пользователя
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModels loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginModel.Email);
            if (user == null)
            {
                Console.WriteLine("Пользователь не найден.");
                return Unauthorized("Неверные данные для входа.");
            }

            // Логируем соль и хеш для отладки
            Console.WriteLine($"Email: {loginModel.Email}");
            Console.WriteLine($"Stored Salt: {user.Salt}");
            var hash = PasswordHelper.ComputeHash(loginModel.Password, user.Salt); // Часть кода
            Console.WriteLine($"Calculated Hash: {hash}");
            Console.WriteLine($"Stored Hash: {user.PasswordHash}");


            if (hash != user.PasswordHash)
            {
                Console.WriteLine("Хеш пороля нне соответствует.");
                return Unauthorized("Неверные данные для входа.");
            }

            // Здесь можно сгенерировать JWT-токен
            return Ok(new { Message = "Вход выполнен успешно", UserId = user.Id, UserLogin = user.Login });
        }
    }
}
