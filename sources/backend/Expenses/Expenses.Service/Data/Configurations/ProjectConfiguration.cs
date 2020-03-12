using AcademyCloud.Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Data.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.OwnsOne(e => e.Quota);

            // Necessary
            builder.Ignore(e => e.PayedOrgTransactions);
            builder.Ignore(e => e.ReceivedOrgTransactions);
            builder.Ignore(e => e.UseCycleRecords);
            builder.Ignore(e => e.BillingCycleRecords);
        }
    }
}
