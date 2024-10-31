using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RemoteWorkCollaboration.Models;

namespace RemoteWorkCollaboration.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<RegisteredUser> UserTb { get; set; } // Make sure this matches your Users table

        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {

            // Explicitly map RegisteredUser entity to UserTB table
            modelBuilder.Entity<RegisteredUser>().ToTable("UserTb");
        }
    }
}
