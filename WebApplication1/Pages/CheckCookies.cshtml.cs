using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages
{
    public class CheckCookiesModel : PageModel
    {
        public string CookieValue { get; set; }

        public IActionResult OnGet()
        {
            CookieValue = Request.Cookies["MyValue"];
            return Page();
        }
    }
}
