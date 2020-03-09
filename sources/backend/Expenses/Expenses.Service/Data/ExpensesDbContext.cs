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
    public class ExpensesDbContext: DbContext
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

        public ExpensesDbContext(DbContextOptions<ExpensesDbContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Setup ValueObjects
            modelBuilder.Entity<UserProjectAssignment>()
                .OwnsOne(e => e.Quota);

            modelBuilder.Entity<Project>()
                .OwnsOne(e => e.Quota);

            modelBuilder.Entity<Project>()
                .Ignore(e => e.SubjectType);

            modelBuilder.Entity<User>()
                .Ignore(e => e.SubjectType);

            modelBuilder.Entity<DomainEntity>()
                .Ignore(e => e.SubjectType);
                

            modelBuilder.Entity<DomainEntity>()
                .OwnsOne(e => e.Quota);

            modelBuilder.Entity<UserTransaction>()
                .OwnsOne(e => e.Reason);

            modelBuilder.Entity<OrgTransaction>()
                .OwnsOne(e => e.Reason);

            modelBuilder.Entity<OrgTransaction>()
                .OwnsOne(e => e.Payer);

            modelBuilder.Entity<OrgTransaction>()
                .OwnsOne(e => e.Receiver);

            modelBuilder.Entity<BillingCycle>()
                .OwnsOne(e => e.Resources);

            modelBuilder.Entity<UseCycle>()
                .OwnsOne(e => e.Resources);
        }
    }
}
