using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShriGo.Pages
{
    public class SignInModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            //verify the login credentials and allow user to profile page 

            return RedirectToPage();
        }
    }
}
