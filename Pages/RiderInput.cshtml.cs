using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriGo.Models;
using System.ComponentModel.DataAnnotations;
using TesingProject.Model;

namespace TesingProject.Pages
{
    public class RiderInputModel : PageModel
    {
        private readonly RideDBContext _dbContext;

        public List<RideModel> listRideModel = new List<RideModel>();
        private DateTime lastStartDate;

        [BindProperty]
        public RideModel NewRideModel { get; set; }


        //Constructor 
        public RiderInputModel(RideDBContext context)
        {
            _dbContext = context;
        }

        public void OnGet()
        {

            listRideModel = _dbContext.DbRides.ToList();
        }

        public IActionResult OnPost()
        {
            // Define the cutoff date
            var cutoffDate = DateOnly.FromDateTime(DateTime.Today); ;
          

            // Finds the entities to remove
            var oldRides = _dbContext.DbRides.Where(r => r.RideDate < cutoffDate).ToList();

            // Remove the entities from the DbSet
            _dbContext.DbRides.RemoveRange(oldRides);

            _dbContext.DbRides.Add(NewRideModel);

            _dbContext.SaveChanges();

            return RedirectToPage();
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
