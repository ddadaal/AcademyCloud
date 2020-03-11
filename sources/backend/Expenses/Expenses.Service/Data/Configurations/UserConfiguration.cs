using AcademyCloud.Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AcademyCloud.Shared.Constants;

namespace AcademyCloud.Expenses.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(new { Id = SocialDomainAdminId, Balance = 0m, ReceiverId = SocialDomainAdminId, Active = true });
            builder.HasData(new { Id = SystemUserId, Balance = 0m, ReceiverId = SystemUserId, Active = true });

            builder.HasMany(e => e.ReceivedUserTransactions).WithOne(e => e.Receiver).IsRequired();
            builder.HasMany(e => e.PayedUserTransactions).WithOne(e => e.Payer);

            builder.Ignore(e => e.PayedOrgTransactions);
        }
    }
}
