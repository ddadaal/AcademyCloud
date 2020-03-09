using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using DomainEntity = AcademyCloud.Expenses.Domain.Entities.Domain;
using SystemEntity = AcademyCloud.Expenses.Domain.Entities.System;

namespace AcademyCloud.Expenses.Data
{
    public class ExpensesDbContext : DbContext
    {
        public DbSet<UserTransaction> UserTransactions { get; set; }

        public DbSet<OrgTransaction> OrgTransactions { get; set; }

        public DbSet<SystemEntity> Systems { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<DomainEntity> Domains { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<UserProjectAssignment> UserProjectAssignments { get; set; }

        public DbSet<UseCycle> UseCycles { get; set; }

        public DbSet<BillingCycle> BillingCycles { get; set; }

        public ExpensesDbContext(DbContextOptions<ExpensesDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the models

            modelBuilder.Entity<DomainEntity>(o =>
            {
                o.Ignore(e => e.Active);
                o.HasOne(e => e.Payer).WithMany(e => e.Domains);
            });
            
            modelBuilder.Entity<UserProjectAssignment>(o =>
            {
                o.OwnsOne(e => e.Quota);
                o.Ignore(e => e.Resources);
            });

            modelBuilder.Entity<Project>(o =>
            {
                o.OwnsOne(e => e.Quota);
                o.Ignore(e => e.SubjectType);
                o.Ignore(e => e.Resources);
                o.Ignore(e => e.Active);
            });

            modelBuilder.Entity<User>(o =>
            {
                o.Ignore(e => e.SubjectType);
            });

            modelBuilder.Entity<DomainEntity>(o =>
            {
                o.OwnsOne(e => e.Quota);
                o.Ignore(e => e.SubjectType);
                o.Ignore(e => e.Resources);
            });

            modelBuilder.Entity<UserTransaction>(o =>
            {
                o.OwnsOne(e => e.Reason);
                o.HasOne(e => e.Payer).WithMany(e => e.PayedUserTransactions);
                o.HasOne(e => e.Receiver).WithMany(e => e.ReceivedUserTransactions);
            });

            modelBuilder.Entity<OrgTransaction>(o =>
            {
                o.OwnsOne(e => e.Reason);
                o.OwnsOne(e => e.Payer);
                o.OwnsOne(e => e.Receiver);
            });

            modelBuilder.Entity<BillingCycle>(o =>
            {
                o.OwnsOne(e => e.Resources);
            });

            modelBuilder.Entity<UseCycle>(o =>
            {
                o.OwnsOne(e => e.Resources);
            });

        }
    }
}
