using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShriGo.Model;

namespace ShriGo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RideApiController : ControllerBase
    {
        private readonly RideDBContext _dbContext;

        public RideApiController(RideDBContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRides()
        {
            var rides = await _dbContext
                .Ride_DBTable
                .ToListAsync();

            return Ok(rides);
        }
    }
}