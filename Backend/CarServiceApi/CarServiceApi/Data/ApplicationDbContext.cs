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

    }
}
