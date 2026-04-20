using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShriGo.Pages
{
    public class SignOutModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> Logout()
        {
            // Clears the session data
            HttpContext.Session.Clear();

            // Signs the user out of the authentication scheme
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
    
    }
