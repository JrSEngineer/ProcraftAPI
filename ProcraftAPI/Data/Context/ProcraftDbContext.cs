using Microsoft.EntityFrameworkCore;
using ProcraftAPI.Entities.Joins;
using ProcraftAPI.Entities.Process;
using ProcraftAPI.Entities.Process.Scope;
using ProcraftAPI.Entities.Process.Step;
using ProcraftAPI.Entities.User;
using ProcraftAPI.Security.Authentication;

namespace ProcraftAPI.Data.Context
{
    public class ProcraftDbContext : DbContext
    {
        public ProcraftDbContext(DbContextOptions<ProcraftDbContext> options) : base(options)
        {
            bool pendingMigrationsFound = this.Database.GetPendingMigrations().Any();

            if (pendingMigrationsFound) this.Database.Migrate();
        }

        public DbSet<ProcraftAuthentication> Authentication { get; set; }
        public DbSet<ProcraftUser> User { get; set; }
        public DbSet<UserAddress> Address { get; set; }
        public DbSet<ProcraftProcess> Process { get; set; }
        public DbSet<ProcessScope> Scope { get; set; }
        public DbSet<ScopeAbility> Ability { get; set; }
        public DbSet<StepUser> StepUser { get; set; }
        public DbSet<ProcessStep> Step {  get; set; }
        public DbSet<ProcessAction> Action { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ProcraftAuthentication>().HasKey(a => a.Email);

            builder.Entity<ProcessUser>().HasKey(pU => new { pU.UserId, pU.ProcessId });

            builder.Entity<StepUser>().HasKey(sU => new { sU.UserId, sU.StepId});
        }
    }
}
