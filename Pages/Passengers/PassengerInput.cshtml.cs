using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ShriGo.Pages.Passengers
{
    public class PassengerInputModel : PageModel
    {
        private DateTime lastStartDate;

        public void OnGet()
        {
        }



        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime LastStartDate
        {
            get { return lastStartDate; }
            set { lastStartDate = value; }
        }
    }
}
