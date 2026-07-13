using Microsoft.EntityFrameworkCore;
using CarServiceApi.Models;

namespace CarServiceApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ServiceLog> ServiceLogs { get; set; }
        public DbSet<FuelLog> FuelLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Vehicle>()
                .HasMany(s => s.ServiceLogs)
                .WithOne(v => v.Vehicle)
                .HasForeignKey(s => s.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Vehicle>()
                .HasMany(f => f.FuelLogs)
                .WithOne(v => v.Vehicle)
                .HasForeignKey(s => s.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FuelLog>()
                .Property(f => f.FuelCost)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ServiceLog>()
                .Property(s => s.ServiceCost)
                .HasPrecision(10, 2);
        }
    }
}
