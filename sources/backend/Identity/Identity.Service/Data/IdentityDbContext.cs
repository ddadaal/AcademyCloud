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
            modelBuilder.Entity<Domain>().HasData(
                new Domain(SocialDomainId, "Social")
            );
        }
    }
}
