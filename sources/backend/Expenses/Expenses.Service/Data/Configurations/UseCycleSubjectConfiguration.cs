using AcademyCloud.Expenses.Domain.Entities;
using AcademyCloud.Expenses.Domain.Entities.UseCycle;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Data.Configurations
{
    public class UseCycleSubjectConfiguration : IEntityTypeConfiguration<UseCycleSubject>
    {
        public void Configure(EntityTypeBuilder<UseCycleSubject> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedNever();

            // Not care about subject. It's not a aggregate root and will not be accessed directly.
            builder.HasMany(x => x.UseCycleRecords).WithOne();

            // Configuration one-to-one relationship of each column
            builder
                .HasOne(x => x.Domain)
                .WithOne(x => x.UseCycleSubject)
                .HasForeignKey<UseCycleSubject>("DomainId");

            builder
                .HasOne(x => x.Project)
                .WithOne(x => x.UseCycleSubject)
                .HasForeignKey<UseCycleSubject>("ProjectId");
            builder
                .HasOne(x => x.UserProjectAssignment)
                .WithOne(x => x.UseCycleSubject)
                .HasForeignKey<UseCycleSubject>("UserProjectAssignmentId");

            // add the social domain into the use cycle.
            builder
                .HasData(new
                {
                    Id = Shared.Constants.SocialDomainId,
                    DomainId = Shared.Constants.SocialDomainId,
                    SubjectType = AcademyCloud.Expenses.Domain.ValueObjects.SubjectType.Domain
                });

        }
    }
}
