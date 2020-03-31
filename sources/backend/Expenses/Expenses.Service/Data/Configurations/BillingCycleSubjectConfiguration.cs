using AcademyCloud.Expenses.Domain.Entities;
using AcademyCloud.Expenses.Domain.Entities.BillingCycle;
using AcademyCloud.Expenses.Domain.Entities.BillingCycle;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Data.Configurations
{
    public class BillingCycleSubjectConfiguration : IEntityTypeConfiguration<BillingCycleSubject>
    {
        public void Configure(EntityTypeBuilder<BillingCycleSubject> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedNever();
            
            // Not care about subject. It's not a aggregate root and will not be accessed directly.
            builder.HasMany(x => x.BillingCycleRecords).WithOne();

            // Necessary
            builder.Ignore(x => x.PayedOrgTransactions);

            // Configuration one-to-one relationship of each column
            builder
                .HasOne(x => x.Domain)
                .WithOne(x => x.BillingCycleSubject)
                .HasForeignKey<BillingCycleSubject>("DomainId")
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.Project)
                .WithOne(x => x.BillingCycleSubject)
                .HasForeignKey<BillingCycleSubject>("ProjectId")
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.UserProjectAssignment)
                .WithOne(x => x.BillingCycleSubject)
                .HasForeignKey<BillingCycleSubject>("UserProjectAssignmentId")
                .OnDelete(DeleteBehavior.Cascade);
                
        }
    }
}
