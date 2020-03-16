using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static AcademyCloud.Shared.Constants;
using AcademyCloud.Expenses.Domain.ValueObjects;
using static AcademyCloud.Expenses.Data.ExpensesDbContext;

namespace AcademyCloud.Expenses.Data.Configurations
{
    public class DomainConfiguration : IEntityTypeConfiguration<Domain.Entities.Domain>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Domain> builder)
        {
            builder
                .OwnsOne(e => e.Quota)
                // seed the social domain's quota
                .HasData(new
                {
                    DomainId = SocialDomainId,
                    Cpu = 0,
                    Memory = 0,
                    Storage = 0
                });


            // Necessary
            builder.Ignore(e => e.PayedOrgTransactions);
            builder.Ignore(e => e.ReceivedOrgTransactions);
            builder.Ignore(e => e.UseCycleRecords);
            builder.Ignore(e => e.BillingCycleRecords);

            // init social domain
            builder.HasData(new 
            { 
                Id = SocialDomainId, 
                PayerId = SocialDomainId, 
                ReceiverId = SocialDomainId, 
                PayUserId = SocialDomainAdminId, 
                SystemId = SystemId,
            });
        }
    }
}
