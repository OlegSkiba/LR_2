using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    [ApiController]
    [Route("Library")] // Встановлення шляху маршрутизації
    public class LibraryController : ControllerBase
    {
        [HttpGet] // Вказуємо, що цей метод відповідає на HTTP GET-запити
        public ActionResult<string> Get()
        {
            return "Hello from the library!";
        }
    }
}
