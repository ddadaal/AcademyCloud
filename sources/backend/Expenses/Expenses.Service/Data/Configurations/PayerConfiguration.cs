using AcademyCloud.Expenses.Domain.Entities;
using AcademyCloud.Expenses.Domain.Entities.Transaction;
using AcademyCloud.Expenses.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AcademyCloud.Shared.Constants;

namespace AcademyCloud.Expenses.Data.Configurations
{
    public class PayerConfiguration : IEntityTypeConfiguration<Payer>
    {
        public void Configure(EntityTypeBuilder<Payer> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.HasMany(e => e.PayedOrgTransactions).WithOne(e => e.Payer);

            // Configure one-to-one relationships
            // Must configure the foreign key to 
            // https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key#one-to-one
            // Foreign key is configured on the child side
            builder.HasOne(e => e.Domain).WithOne(e => e.Payer).HasForeignKey<Payer>("DomainId");
            builder.HasOne(e => e.Project).WithOne(e => e.Payer).HasForeignKey<Payer>("ProjectId");
            builder.HasOne(e => e.User).WithOne(e => e.Payer).HasForeignKey<Payer>("UserId");

            // Setup payer for social domain admin and system user
            builder.HasData(new { Id = SocialDomainAdminId, UserId = SocialDomainAdminId, SubjectType = SubjectType.User });
            builder.HasData(new { Id = SystemUserId, UserId = SystemUserId, SubjectType = SubjectType.User });

            // Setup payer for social domain
            builder.HasData(new { Id = SocialDomainId, DomainId = SocialDomainId, SubjectType = SubjectType.Domain });
        }
    }
}
