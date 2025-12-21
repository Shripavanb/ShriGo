using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TesingProject.Model;

namespace ShriGo.Models
{
    public class RideDBContext : DbContext
    {
        public DbSet<RideModel> DbRides { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Auto increment of ID 
            modelBuilder.Entity<RideModel>()
            .Property(p => p.RideId)
            .ValueGeneratedOnAdd(); // Configures the property to have its value generated on add
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           optionsBuilder.UseInMemoryDatabase("RideDb");
        }

    }
}
