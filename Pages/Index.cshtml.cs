using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriGo.Model;


namespace ShriGo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly RideDBContext _dbContext;

        public List<RideModel> listRideModel = new List<RideModel>();


        public IndexModel(ILogger<IndexModel> logger, RideDBContext context)
        {
            _logger = logger;
            _dbContext = context;
        }

        public void OnGet()
        {
            // Define the cutoff date, date only 
            var cutoffDate = DateOnly.FromDateTime(DateTime.Today);

            // Finds the entities to remove
            var oldRides = _dbContext.RideDBTable.Where(r => r.RideDate < cutoffDate).ToList();

            // Make sure every time it Removes the old entities from the DbSet
            _dbContext.RideDBTable.RemoveRange(oldRides);

            //Finally Arrange list as per date and time 
            listRideModel = _dbContext.RideDBTable.OrderBy(x => x.RideDate).ThenBy(x => x.RideTime).ToList();
        }
    }
}
