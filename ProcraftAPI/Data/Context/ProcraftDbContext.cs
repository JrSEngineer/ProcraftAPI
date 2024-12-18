﻿using Microsoft.EntityFrameworkCore;
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
        public DbSet<AccountRecovery> Recovery { get; set; }
        public DbSet<ProcraftGroup> Group { get; set; }
        public DbSet<ProcraftUser> User { get; set; }
        public DbSet<UserAddress> Address { get; set; }
        public DbSet<ProcraftProcess> Process { get; set; }
        public DbSet<ProcessScope> Scope { get; set; }
        public DbSet<ScopeAbility> Ability { get; set; }
        public DbSet<StepUser> StepUser { get; set; }
        public DbSet<ProcessStep> Step { get; set; }
        public DbSet<ProcessAction> Action { get; set; }
        public DbSet<ProcessManager> Manager { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ProcraftGroup>().HasMany(g => g.Members).WithOne().HasForeignKey(m => m.GroupId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProcraftGroup>().HasMany(g => g.Managers).WithOne().HasForeignKey(m => m.GroupId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProcraftAuthentication>().HasKey(a => a.Email);

            builder.Entity<AccountRecovery>().HasKey(a => a.TransactionId);

            builder.Entity<ProcraftUser>().HasOne(u => u.Authentication).WithOne(a => a.User).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProcraftUser>().HasMany(u => u.Steps).WithMany(s => s.Users).UsingEntity<StepUser>();

            builder.Entity<ProcraftUser>().HasMany(u => u.Actions).WithOne().HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProcraftUser>().HasOne(u => u.Manager).WithOne(m => m.User).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProcessManager>().HasMany(p => p.Processes).WithOne(m => m.Manager).HasForeignKey(p => p.ManagerId).IsRequired();

            builder.Entity<ProcraftProcess>().HasMany(p => p.Users).WithMany(s => s.Processes).UsingEntity<ProcessUser>();

            builder.Entity<ProcraftProcess>().HasMany(p => p.Steps).WithOne(s => s.Process).HasForeignKey(s => s.ProcessId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProcessStep>().HasMany(u => u.Actions).WithOne().HasForeignKey(a => a.StepId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProcessUser>().HasKey(pU => new { pU.UserId, pU.ProcessId });

            builder.Entity<StepUser>().HasKey(sU => new { sU.UserId, sU.StepId });
        }
    }
}
