using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShriGo.Pages
{
    public class RiderProfileModel : PageModel
    {
        public void OnGet()
        {
            string userValue = HttpContext.Session.GetString("UserName");
        }
    }
}
