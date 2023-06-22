using Core.Models;
using Core.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Core.DATABASE
{
    public class AppDbContext:IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUser { get; set; } 
        public DbSet<Food> Foods { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<JobApplication> JobApplication { get; set; }
        public DbSet<WorkDetails> WorkDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<SalesRecord> SalesRecords { get; set; }
    }
}
