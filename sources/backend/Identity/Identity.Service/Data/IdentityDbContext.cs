using AcademyCloud.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using DomainEntity = AcademyCloud.Identity.Domain.Entities.Domain;
using static AcademyCloud.Shared.Constants;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AcademyCloud.Identity.Data
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<DomainEntity> Domains { get; set; }

        public DbSet<UserDomainAssignment> UserDomainAssignments { get; set; }

        public DbSet<UserProjectAssignment> UserProjectAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var socialDomain = new DomainEntity(SocialDomainId, "Social");
            var socialDomainAdmin = new User(SocialDomainAdminId, "socialadmin", "Social Admin", "123", "socialadmin@ac.com", false);
            var systemUser = new User(SystemUserId, "system", "system1", "system", "system@ac.com", true); 

            modelBuilder.Entity<UserDomainAssignment>()
                .HasData(new { Id = Guid.NewGuid(), UserId = SocialDomainAdminId, DomainId = SocialDomainId, Role = Domain.ValueObjects.UserRole.Admin });

            modelBuilder.Entity<User>(o =>
            {
                // init social domain user and system user
                o.HasData(socialDomainAdmin);
                o.HasData(systemUser);
                o.HasIndex(x => x.Username).IsUnique();
            });

            modelBuilder.Entity<DomainEntity>(o =>
            {
                // init social domain
                o.HasData(socialDomain);
                o.HasIndex(x => x.Name).IsUnique();
            });

            // Set DateTime to UTC
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ? v.Value.ToUniversalTime() : v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.IsKeyless)
                {
                    continue;
                }

                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(nullableDateTimeConverter);
                    }
                }
            }

        }
    }
}
