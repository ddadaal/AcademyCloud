using AcademyCloud.Expenses.Domain.Entities.Transaction;
using AcademyCloud.Expenses.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AcademyCloud.Shared.Constants;
using static AcademyCloud.Expenses.Data.ExpensesDbContext;
using AcademyCloud.Expenses.Domain.Entities;

namespace AcademyCloud.Expenses.Data.Configurations
{
    public class ReceiverConfiguration : IEntityTypeConfiguration<Receiver>
    {
        public void Configure(EntityTypeBuilder<Receiver> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.HasMany(e => e.ReceivedOrgTransactions).WithOne(e => e.Receiver);

            // Configure one-to-one relationships
            // Add a DomainId foreign key on Receiver (HasForeignKey) referencing Domain
            builder.HasOne(e => e.Domain).WithOne(e => e.Receiver).HasForeignKey<Receiver>("DomainId");
            builder.HasOne(e => e.Project).WithOne(e => e.Receiver).HasForeignKey<Receiver>("ProjectId");
            builder.HasOne(e => e.System).WithOne(e => e.Receiver).HasForeignKey<Receiver>("SystemId");

            // Setup receiver for System and Social Domain
            builder.HasData(new { Id = SystemId, SystemId = SystemId, SubjectType = SubjectType.System });
            builder.HasData(new { Id = SocialDomainId, DomainId = SocialDomainId, SubjectType = SubjectType.Domain });
        }
    }
}
