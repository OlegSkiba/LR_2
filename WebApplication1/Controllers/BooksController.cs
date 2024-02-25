using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace WebApplication1
{
    [ApiController]
    [Route("Library/Books")] // Встановлення шляху маршрутизації
    public class BooksController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public BooksController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet] // Вказуємо, що цей метод відповідає на HTTP GET-запити
        public ActionResult<IEnumerable<string>> GetBooks()
        {
            // Отримуємо дані про книги з файлу конфігурації
            var books = _configuration.GetSection("Library:Books").Get<List<string>>();

            if (books == null)
            {
                return NotFound(); // Повертаємо 404 Not Found, якщо список книг порожній або не знайдений
            }

            return Ok(books);
        }
    }
}
