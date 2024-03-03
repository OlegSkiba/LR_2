using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public string Value { get; set; }

        [BindProperty]
        public string Date { get; set; }

        public IActionResult OnPost()
        {
            // Parse date from string
            if (DateTime.TryParse(Date, out DateTime expirationDate))
            {
                // Set value in cookies with expiration date equal to Date
                Response.Cookies.Append("MyValue", Value, new CookieOptions
                {
                    Expires = expirationDate
                });
                return RedirectToPage();
            }
            else
            {
                // Handle invalid date format
                return Page();
            }
        }
    }
}
