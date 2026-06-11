using Microsoft.EntityFrameworkCore;
using CarServiceApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CarServiceApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ServiceLog> ServiceLogs { get; set; }
        public DbSet<FuelLog> FuelLogs { get; set; }    

    }
}
