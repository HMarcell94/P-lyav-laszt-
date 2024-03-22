using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PalyavalasztoV2.Models.main;

namespace PalyavalasztoV2.Data
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

            builder.Entity<PalyavalasztoV2.Models.main.Application>()
              .HasOne(i => i.employee)
              .WithMany(i => i.applications)
              .HasForeignKey(i => i.EmployeeId)
              .HasPrincipalKey(i => i.EmployeeId);

            builder.Entity<PalyavalasztoV2.Models.main.Application>()
              .HasOne(i => i.role)
              .WithMany(i => i.applications)
              .HasForeignKey(i => i.EmployeeId)
              .HasPrincipalKey(i => i.RoleID);

            builder.Entity<PalyavalasztoV2.Models.main.Application>()
              .HasOne(i => i.supportrole)
              .WithMany(i => i.applications)
              .HasForeignKey(i => i.EmployeeId)
              .HasPrincipalKey(i => i.UserID);

            builder.Entity<PalyavalasztoV2.Models.main.Application>()
              .HasOne(i => i.job)
              .WithMany(i => i.applications)
              .HasForeignKey(i => i.JobId)
              .HasPrincipalKey(i => i.JobID);

            builder.Entity<PalyavalasztoV2.Models.main.Employee>()
              .HasOne(i => i.adminrole)
              .WithMany(i => i.employees)
              .HasForeignKey(i => i.EmployeeId)
              .HasPrincipalKey(i => i.UserId);

            builder.Entity<PalyavalasztoV2.Models.main.Employee>()
              .HasOne(i => i.userrole)
              .WithMany(i => i.employees)
              .HasForeignKey(i => i.EmployeeId)
              .HasPrincipalKey(i => i.UserId);

            builder.Entity<PalyavalasztoV2.Models.main.Employee>()
              .HasOne(i => i.employer)
              .WithMany(i => i.employees)
              .HasForeignKey(i => i.EmployeeId)
              .HasPrincipalKey(i => i.id);

            builder.Entity<PalyavalasztoV2.Models.main.Employee>()
              .HasOne(i => i.location)
              .WithMany(i => i.employees)
              .HasForeignKey(i => i.LocationId)
              .HasPrincipalKey(i => i.LocationId);

            builder.Entity<PalyavalasztoV2.Models.main.Employee>()
              .HasOne(i => i.role)
              .WithMany(i => i.employees)
              .HasForeignKey(i => i.RoleId)
              .HasPrincipalKey(i => i.RoleID);

            builder.Entity<PalyavalasztoV2.Models.main.Job>()
              .HasOne(i => i.employer)
              .WithMany(i => i.jobs)
              .HasForeignKey(i => i.EmployerId)
              .HasPrincipalKey(i => i.id);

            builder.Entity<PalyavalasztoV2.Models.main.Supportrole>()
              .HasOne(i => i.role)
              .WithMany(i => i.supportroles)
              .HasForeignKey(i => i.RoleID)
              .HasPrincipalKey(i => i.RoleID);

            builder.Entity<PalyavalasztoV2.Models.main.Location>()
              .Property(p => p.Latttude)
              .HasPrecision(10,0);

            builder.Entity<PalyavalasztoV2.Models.main.Location>()
              .Property(p => p.Longitude)
              .HasPrecision(10,0);
            this.OnModelBuilding(builder);
        }

        public DbSet<PalyavalasztoV2.Models.main.Adminrole> adminroles { get; set; }

        public DbSet<PalyavalasztoV2.Models.main.Application> applications { get; set; }

        public DbSet<PalyavalasztoV2.Models.main.Employee> employees { get; set; }

        public DbSet<PalyavalasztoV2.Models.main.Employer> employers { get; set; }

        public DbSet<PalyavalasztoV2.Models.main.Job> jobs { get; set; }

        public DbSet<PalyavalasztoV2.Models.main.Location> locations { get; set; }

        public DbSet<PalyavalasztoV2.Models.main.Role> roles { get; set; }

        public DbSet<PalyavalasztoV2.Models.main.Supportrole> supportroles { get; set; }

        public DbSet<PalyavalasztoV2.Models.main.Userrole> userroles { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    
    }
}