using AcademyCloud.Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Data.Configurations
{
    public class UserProjectAssignmentConfiguration : IEntityTypeConfiguration<UserProjectAssignment>
    {
        public void Configure(EntityTypeBuilder<UserProjectAssignment> builder)
        {
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.OwnsOne(e => e.Quota);
            builder.OwnsOne(e => e.Resources);

            builder.Ignore(e => e.PayedOrgTransactions);
            builder.Ignore(e => e.UseCycleRecords);
            builder.Ignore(e => e.BillingCycleRecords);

            // Relationship to UseCycleSubject has been configured on the UseCycleSubject
        }
    }
}
