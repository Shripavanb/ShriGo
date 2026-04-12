using Microsoft.EntityFrameworkCore;

namespace ShriGo.Model
{
    public class DriverDBContext: DbContext
    {
        public DriverDBContext(DbContextOptions<DriverDBContext> options) : base(options)
        {
        }

        public DbSet<DriverModel> DriversTb { get; set; }


    }
}
