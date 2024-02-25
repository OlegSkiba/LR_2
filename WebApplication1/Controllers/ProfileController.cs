using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebApplication1
{
    [ApiController]
    [Route("Library/Profile")] // Встановлення шляху маршрутизації
    public class ProfileController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProfileController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{id?}")] // Вказуємо, що цей метод відповідає на HTTP GET-запити з необов'язковим параметром id
        public ActionResult<string> GetUserProfile(int? id)
        {
            // Отримуємо інформацію про користувача з файлу конфігурації
            string sectionName = id.HasValue ? $"Users:User{id}" : "Users:CurrentUser";
            var userProfile = _configuration.GetValue<string>(sectionName);

            if (userProfile == null)
            {
                return NotFound(); // Повертаємо 404 Not Found, якщо інформація про користувача не знайдена
            }

            return Ok(userProfile);
        }
    }
}
