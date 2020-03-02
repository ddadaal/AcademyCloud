using AcademyCloud.Identity.Domains.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Identity.Data
{
    public class IdentityDbContext : DbContext
    {

        public static readonly Guid SocialDomainId = new Guid("A7476756-858C-44C5-BC77-4C41212B364D");
        public static readonly Guid SocialDomainAdminId = new Guid("C910EA06-E8FC-4D8F-A9D0-434F7CB1949A");
        public static readonly Guid SocialDomainAdminDomainAssignmentId = new Guid("C87D2CE9-EF74-41DE-B736-3EF3182F9B9F");
        public static readonly Guid SystemUserId = new Guid("97B2C3BF-310F-4899-8D75-983A8E5D9894");

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Domain> Domains { get; set; }

        public DbSet<UserDomainAssignment> UserDomainAssignments { get; set; }

        public DbSet<UserProjectAssignment> UserProjectAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var socialDomain = new Domain(SocialDomainId, "Social");
            var socialDomainAdmin = new User(SocialDomainAdminId, "socialadmin", "Social Admin", "123", "socialadmin@ac.com", false);

            modelBuilder.Entity<UserDomainAssignment>()
                .HasData(new { Id = Guid.NewGuid(), UserId = SocialDomainAdminId, DomainId = SocialDomainId, Role = Identity.Domains.ValueObjects.UserRole.Admin });

            // initial social domain
            modelBuilder.Entity<Domain>()
                .HasData(socialDomain);

            // initial the social domain admin
            modelBuilder.Entity<User>()
                .HasData(socialDomainAdmin);


            // initial system user
            modelBuilder.Entity<User>()
                .HasData(new User(SystemUserId, "system", "system1", "system", "system@ac.com", true));

            // domain name uniqueness
            modelBuilder.Entity<Domain>()
                .HasIndex(x => x.Name)
                .IsUnique();

            // username uniqueness
            modelBuilder.Entity<User>()
                .HasIndex(x => x.Username)
                .IsUnique();

        }
    }
}
