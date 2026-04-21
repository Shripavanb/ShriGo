using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShriGo.Pages
{
    public class SignOutModel : PageModel
    {
        //private readonly SignInManager _signInManager;
        public async Task<IActionResult> OnGet()
        {
            // _signInManager.IsSignedIn(User);

            // Clears the session data
            HttpContext.Session.Clear();

            // Signs the user out of the authentication scheme
           // await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> SignOut()
        {
            // Clears the session data
            HttpContext.Session.Clear();

            // Signs the user out of the authentication scheme
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("/Index", "Home");
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
           // await _signInManager.SignOutAsync(); // Logs the user out
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            return RedirectToPage("/Index"); // Redirect to home after logout
        }
    }
    
    }
