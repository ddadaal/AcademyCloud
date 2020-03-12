using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using DomainEntity = AcademyCloud.Expenses.Domain.Entities.Domain;
using SystemEntity = AcademyCloud.Expenses.Domain.Entities.System;
using static AcademyCloud.Shared.Constants;
using AcademyCloud.Expenses.Domain.ValueObjects;
using AcademyCloud.Expenses.Domain.Entities.Transaction;
using AcademyCloud.Expenses.Domain.Entities.UseCycle;
using AcademyCloud.Expenses.Data.Configurations;
using AcademyCloud.Expenses.Domain.Entities.ManagementFee;

namespace AcademyCloud.Expenses.Data
{
    public class ExpensesDbContext : DbContext
    {
        public static readonly Guid SystemGuid = Guid.Parse("FB7D021C-7284-43C5-92CA-F1164F61B808");

        public DbSet<UserTransaction> UserTransactions { get; set; }

        public DbSet<OrgTransaction> OrgTransactions { get; set; }

        public DbSet<SystemEntity> Systems { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<DomainEntity> Domains { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<UserProjectAssignment> UserProjectAssignments { get; set; }

        public DbSet<ManagementFeeEntry> ManagementFeeEntries { get; set; }

        public DbSet<UseCycleEntry> UseCycleEntries { get; set; }

        public ExpensesDbContext(DbContextOptions<ExpensesDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Payer and Receiver are proxies to the real Payer and Receiver
            // Payer and Receiver has a column for each concrete Payer and Receiver class
            // Payer and Receiver share id from their concrete class
            modelBuilder.ApplyConfiguration(new PayerConfiguration());
            modelBuilder.ApplyConfiguration(new ReceiverConfiguration());


            modelBuilder.Entity<SystemEntity>(o =>
            {
                // init system
                o.HasData(new { Id = SystemGuid, ReceiveUserId = SystemUserId, ReceiverId = SystemGuid });

                o.Ignore(e => e.ReceivedOrgTransactions);
            });


            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new DomainConfiguration());

            modelBuilder.Entity<UserDomainAssignment>(o =>
            {
                o.Property(e => e.Id).ValueGeneratedNever();

            });

            modelBuilder.ApplyConfiguration(new UserProjectAssignmentConfiguration());

            modelBuilder.ApplyConfiguration(new ProjectConfiguration());

            modelBuilder.Entity<UserTransaction>(o =>
            {
                o.OwnsOne(e => e.Reason);
                o.HasOne(e => e.Payer).WithMany(e => e.PayedUserTransactions);
                o.HasOne(e => e.Receiver).WithMany(e => e.ReceivedUserTransactions).IsRequired();
                o.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<OrgTransaction>(o =>
            {
                o.Property(e => e.Id).ValueGeneratedNever();
                o.OwnsOne(e => e.Reason);
            });

            modelBuilder.Entity<BillingCycle>(o =>
            {
                o.OwnsOne(e => e.Resources);
            });

            modelBuilder.Entity<UseCycleRecord>(o =>
            {
                o.OwnsOne(e => e.Resources);
            });

            modelBuilder.ApplyConfiguration(new UseCycleSubjectConfiguration());

        }
    }
}
