using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriGo.Model;
using System.ComponentModel.DataAnnotations;


namespace ShriGo.Pages
{
    public class RiderInputModel : PageModel
    {
        private readonly RideDBContext _dbContext;

        [BindProperty]
        public int newRideId { get ; set ;}

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
            listRideModel = _dbContext.RideDBTable.ToList();
        }

        public IActionResult OnPost()
        {
     
            // Define the cutoff date
            var cutoffDate = DateTime.Today ;
          

            // Finds the entities to remove
            var oldRides = _dbContext.RideDBTable.Where(r => r.RideDate < cutoffDate).ToList();

            // Finds the max Id number and adds +1 to it 
            var newRideId = _dbContext.RideDBTable.Max(r => r.RideId);

            // Remove the entities from the DbSet
            _dbContext.RideDBTable.RemoveRange(oldRides);

            NewRideModel.RideId = newRideId+1;
            _dbContext.RideDBTable.Add(NewRideModel);

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
