using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PalyavalsztoV3.Models.main;

namespace PalyavalsztoV3.Data
{
    public partial class mainContext : DbContext
    {
        public mainContext()
        {
        }

        public mainContext(DbContextOptions<mainContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PalyavalsztoV3.Models.main.application>()
              .HasOne(i => i.employee)
              .WithMany(i => i.applications)
              .HasForeignKey(i => i.EmployeeId)
              .HasPrincipalKey(i => i.EmployeeId);

            builder.Entity<PalyavalsztoV3.Models.main.application>()
              .HasOne(i => i.role)
              .WithMany(i => i.applications)
              .HasForeignKey(i => i.EmployeeId)
              .HasPrincipalKey(i => i.RoleID);

            builder.Entity<PalyavalsztoV3.Models.main.application>()
              .HasOne(i => i.supportrole)
              .WithMany(i => i.applications)
              .HasForeignKey(i => i.EmployeeId)
              .HasPrincipalKey(i => i.userID);

            builder.Entity<PalyavalsztoV3.Models.main.application>()
              .HasOne(i => i.job)
              .WithMany(i => i.applications)
              .HasForeignKey(i => i.JobId)
              .HasPrincipalKey(i => i.JobID);

            builder.Entity<PalyavalsztoV3.Models.main.employee>()
              .HasOne(i => i.adminrole)
              .WithMany(i => i.employees)
              .HasForeignKey(i => i.EmployeeId)
              .HasPrincipalKey(i => i.UserId);

            builder.Entity<PalyavalsztoV3.Models.main.employee>()
              .HasOne(i => i.userrole)
              .WithMany(i => i.employees)
              .HasForeignKey(i => i.EmployeeId)
              .HasPrincipalKey(i => i.UserId);

            builder.Entity<PalyavalsztoV3.Models.main.employee>()
              .HasOne(i => i.employer)
              .WithMany(i => i.employees)
              .HasForeignKey(i => i.EmployeeId)
              .HasPrincipalKey(i => i.id);

            builder.Entity<PalyavalsztoV3.Models.main.employee>()
              .HasOne(i => i.location)
              .WithMany(i => i.employees)
              .HasForeignKey(i => i.LocationId)
              .HasPrincipalKey(i => i.LocationId);

            builder.Entity<PalyavalsztoV3.Models.main.employee>()
              .HasOne(i => i.role)
              .WithMany(i => i.employees)
              .HasForeignKey(i => i.RoleId)
              .HasPrincipalKey(i => i.RoleID);

            builder.Entity<PalyavalsztoV3.Models.main.job>()
              .HasOne(i => i.employer)
              .WithMany(i => i.jobs)
              .HasForeignKey(i => i.EmployerId)
              .HasPrincipalKey(i => i.id);

            builder.Entity<PalyavalsztoV3.Models.main.supportrole>()
              .HasOne(i => i.role)
              .WithMany(i => i.supportroles)
              .HasForeignKey(i => i.RoleID)
              .HasPrincipalKey(i => i.RoleID);

            builder.Entity<PalyavalsztoV3.Models.main.user>()
              .HasOne(i => i.employee)
              .WithMany(i => i.users)
              .HasForeignKey(i => i.employee_id)
              .HasPrincipalKey(i => i.EmployeeId);

            builder.Entity<PalyavalsztoV3.Models.main.user>()
              .HasOne(i => i.employer)
              .WithMany(i => i.users)
              .HasForeignKey(i => i.employeer_id)
              .HasPrincipalKey(i => i.id);

            builder.Entity<PalyavalsztoV3.Models.main.location>()
              .Property(p => p.Latttude)
              .HasPrecision(10,0);

            builder.Entity<PalyavalsztoV3.Models.main.location>()
              .Property(p => p.Longitude)
              .HasPrecision(10,0);
            this.OnModelBuilding(builder);
        }

        public DbSet<PalyavalsztoV3.Models.main.adminrole> adminroles { get; set; }

        public DbSet<PalyavalsztoV3.Models.main.application> applications { get; set; }

        public DbSet<PalyavalsztoV3.Models.main.employee> employees { get; set; }

        public DbSet<PalyavalsztoV3.Models.main.employer> employers { get; set; }

        public DbSet<PalyavalsztoV3.Models.main.job> jobs { get; set; }

        public DbSet<PalyavalsztoV3.Models.main.location> locations { get; set; }

        public DbSet<PalyavalsztoV3.Models.main.role> roles { get; set; }

        public DbSet<PalyavalsztoV3.Models.main.supportrole> supportroles { get; set; }

        public DbSet<PalyavalsztoV3.Models.main.user> users { get; set; }

        public DbSet<PalyavalsztoV3.Models.main.userrole> userroles { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    
    }
}