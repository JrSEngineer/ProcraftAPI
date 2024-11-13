using Microsoft.EntityFrameworkCore;
using ProcraftAPI.Entities.User;
using ProcraftAPI.Security.Authentication;

namespace ProcraftAPI.Data.Context
{
    public class ProcraftDbContext : DbContext
    {
        public ProcraftDbContext(DbContextOptions<ProcraftDbContext> options) : base(options)
        {

        }

        public DbSet<ProcraftAuthentication> Authentication { get; set; }
        public DbSet<ProcraftUser> User { get; set; }
        public DbSet<UserAddress> Address { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ProcraftAuthentication>().HasKey(a => a.Email);
        }
    }
}
