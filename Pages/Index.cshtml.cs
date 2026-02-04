using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriGo.Models;
using TesingProject.Model;

namespace TesingProject.Pages
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
            listRideModel = _dbContext.RideDBTable.ToList();
        }
    }
}
