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
using AcademyCloud.Expenses.BackgroundTasks.ManagementFee;

namespace AcademyCloud.Expenses.Data
{
    public class ExpensesDbContext : DbContext
    {
        public readonly Guid SystemGuid = Guid.Parse("FB7D021C-7284-43C5-92CA-F1164F61B808");

        public DbSet<UserTransaction> UserTransactions { get; set; }

        public DbSet<OrgTransaction> OrgTransactions { get; set; }

        public DbSet<SystemEntity> Systems { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<DomainEntity> Domains { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<UserProjectAssignment> UserProjectAssignments { get; set; }

        public DbSet<UseCycle> UseCycles { get; set; }

        public DbSet<BillingCycle> BillingCycles { get; set; }

        public DbSet<ManagementFeeEntry> ManagementFeeEntries { get; set; }

        public ExpensesDbContext(DbContextOptions<ExpensesDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Payer and Receiver are proxies to the real Payer and Receiver
            // Payer and Receiver has a column for each concrete Payer and Receiver class
            // Payer and Receiver share id from their concrete class
            modelBuilder.Entity<Payer>(o =>
            {
                o.Property(x => x.Id).ValueGeneratedNever();
                o.HasMany(e => e.PayedOrgTransactions).WithOne(e => e.Payer);

                // Setup payer for social domain admin and system user
                o.HasData(new { Id = SocialDomainAdminId, UserId = SocialDomainAdminId, SubjectType = SubjectType.User});
                o.HasData(new { Id = SystemUserId, UserId = SystemUserId, SubjectType = SubjectType.User });

                // Setup payer for social domain
                o.HasData(new { Id = SocialDomainId, DomainId = SocialDomainId, SubjectType = SubjectType.Domain });
            });

            modelBuilder.Entity<Receiver>(o =>
            {
                o.Property(x => x.Id).ValueGeneratedNever();
                o.HasMany(e => e.ReceivedOrgTransactions).WithOne(e => e.Receiver);

                // Setup receiver for System and Social Domain
                o.HasData(new { Id = SystemGuid, SystemId = SystemGuid, SubjectType = SubjectType.System });
                o.HasData(new { Id = SocialDomainId, DomainId = SocialDomainId, SubjectType = SubjectType.Domain });
                
            });

            modelBuilder.Entity<SystemEntity>(o =>
            {
                // init system
                o.HasData(new { Id = SystemGuid, ReceiveUserId = SystemUserId, ReceiverId = SystemGuid });

                o.HasOne(e => e.Receiver).WithOne(e => e.System).HasForeignKey<Receiver>("SystemId");

                o.Ignore(e => e.ReceivedOrgTransactions);
            });

            modelBuilder.Entity<User>(o =>
            {
                // init social domain admin and system user
                o.HasData(new { Id = SocialDomainAdminId, Balance = 0m, ReceiverId = SocialDomainAdminId, Active = true });
                o.HasData(new { Id = SystemUserId, Balance = 0m, ReceiverId = SystemUserId, Active = true });

                o.HasMany(e => e.ReceivedUserTransactions).WithOne(e => e.Receiver).IsRequired();
                o.HasMany(e => e.PayedUserTransactions).WithOne(e => e.Payer);

                // Must configure the foreign key to 
                // https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key#one-to-one
                // Foreign key is configured on the child side
                o.HasOne(e => e.Payer).WithOne(e => e.User).HasForeignKey<Payer>("UserId");

                o.Ignore(e => e.PayedOrgTransactions);
            });

            modelBuilder.Entity<DomainEntity>(o =>
            {
                o.OwnsOne(e => e.Quota);

                // Necessary
                o.Ignore(e => e.PayedOrgTransactions);
                o.Ignore(e => e.ReceivedOrgTransactions);

                o.HasOne(e => e.Payer).WithOne(e => e.Domain).HasForeignKey<Payer>("DomainId");
                o.HasOne(e => e.Receiver).WithOne(e => e.Domain).HasForeignKey<Receiver>("DomainId");

                // init social domain
                o.HasData(new { Id = SocialDomainId, PayerId = SocialDomainId, ReceiverId = SocialDomainId, PayUserId = SocialDomainAdminId, Resources = Resources.Zero });

            });

            modelBuilder.Entity<UserDomainAssignment>(o =>
            {
                o.Property(e => e.Id).ValueGeneratedNever();

                //o.HasOne(e => e.User).WithMany(e => e.Domains);
                //o.HasOne(e => e.Domain).WithMany(e => e.Users);
            });

            modelBuilder.Entity<UserProjectAssignment>(o =>
            {
                o.Property(e => e.Id).ValueGeneratedNever();
                o.OwnsOne(e => e.Quota);
                o.OwnsOne(e => e.Resources);
            });

            modelBuilder.Entity<Project>(o =>
            {
                o.OwnsOne(e => e.Quota);

                o.HasOne(e => e.Payer).WithOne(e => e.Project).HasForeignKey<Payer>("ProjectId");
                o.HasOne(e => e.Receiver).WithOne(e => e.Project).HasForeignKey<Receiver>("ProjectId");

                // Necessary
                o.Ignore(e => e.PayedOrgTransactions);
                o.Ignore(e => e.ReceivedOrgTransactions);
            });

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

            modelBuilder.Entity<UseCycle>(o =>
            {
                o.OwnsOne(e => e.Resources);
            });

        }
    }
}
