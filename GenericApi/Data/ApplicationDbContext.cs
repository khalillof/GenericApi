using GenericApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GenericApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var cust = builder.Entity<Customer>(); 
            cust.HasKey(p => p.Id);
            cust.Property(c => c.Id).ValueGeneratedOnAdd();
            cust.Property(c => c.Email).IsRequired();
        }

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<ApplicationUser> AppUsers { get; set; } = null!;
    }
}
