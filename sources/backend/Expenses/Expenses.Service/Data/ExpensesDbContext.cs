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

        public ExpensesDbContext(DbContextOptions<ExpensesDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the models
            var systemUser = new User(SystemUserId, 0);
            var socialDomainAdmin = new User(SocialDomainAdminId, 0);

            modelBuilder.Entity<SystemEntity>(o =>
            {
                // init system
                o.HasData(new { Id = SystemGuid, SystemReceiverId = systemUser.Id });

                o.HasMany(e => e.ReceivedOrgTransactions).WithOne(e => e.ReceiverSystem);
            });

            modelBuilder.Entity<User>(o =>
            {
                // init social domain user and system user
                o.HasData(socialDomainAdmin);
                o.HasData(systemUser);

                o.HasMany(e => e.ReceivedUserTransactions).WithOne(e => e.Receiver).IsRequired();
                o.HasMany(e => e.PayedUserTransactions).WithOne(e => e.Payer);
                o.HasMany(e => e.PayedOrgTransactions).WithOne(e => e.PayerUser);
            });

            modelBuilder.Entity<DomainEntity>(o =>
            {
                o.OwnsOne(e => e.Quota);

                o.HasMany(e => e.PayedOrgTransactions).WithOne(e => e.PayerDomain);
                o.HasMany(e => e.ReceivedOrgTransactions).WithOne(e => e.ReceiverDomain);


                // init social domain
                o.HasData(new { Id = SocialDomainId, PayerId = socialDomainAdmin.Id, Resources = Domain.ValueObjects.Resources.Zero });

            });

            modelBuilder.Entity<UserDomainAssignment>(o =>
            {
                o.Property(e => e.Id).ValueGeneratedNever();
                o.HasOne(e => e.User).WithMany(e => e.Domains);
                o.HasOne(e => e.Domain).WithMany(e => e.Users);
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

                o.HasMany(e => e.PayedOrgTransactions).WithOne(e => e.PayerProject);
                o.HasMany(e => e.ReceivedOrgTransactions).WithOne(e => e.ReceiverProject);
            });

            modelBuilder.Entity<Payer>(o =>
            {
                o.HasKey(e => e.RecordId);
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
                o.OwnsOne(e => e.Reason);
                o.Property(e => e.Id).ValueGeneratedNever();

                o.HasOne(e => e.ReceiverSystem).WithMany(e => e.ReceivedOrgTransactions);
                o.HasOne(e => e.ReceiverDomain).WithMany(e => e.ReceivedOrgTransactions);
                o.HasOne(e => e.ReceiverProject).WithMany(e => e.ReceivedOrgTransactions);

                o.HasOne(e => e.PayerUser).WithMany(e => e.PayedOrgTransactions);
                o.HasOne(e => e.PayerProject).WithMany(e => e.PayedOrgTransactions);
                o.HasOne(e => e.PayerDomain).WithMany(e => e.PayedOrgTransactions);
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
