using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;


namespace ShriGo.Model
{
    public class RideDBContext : DbContext
    {

        public RideDBContext(DbContextOptions<RideDBContext> options) : base(options) 
        { }
        public DbSet<RideModel> RideDBTable { get; set; }
        public DbSet<SortedRideModel> Ride_DBTable { get; set; }

        public DbSet<UserModel> UserTb { get; set; }

        public DbSet<BookedRideModel> BookedRide_DBTable { get; set; }

        public DbSet<BookingsModel> Bookings_DBTable { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    //Auto increment of ID 
        //    modelBuilder.Entity<RideModel>()
        //    .Property(p => p.RideId)
        //    .ValueGeneratedOnAdd(); // Configures the property to have its value generated on add
        //    base.OnModelCreating(modelBuilder);
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //optionsBuilder.UseInMemoryDatabase("RideDb");
        //    optionsBuilder.UseSqlServer();
        //}

    }
}
