using AcademyCloud.Expenses.Domain.Entities.UseCycle;
using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
    public class UserProjectAssignment: IUseCycleSubject
    {

        public Guid Id { get; set; }
        public virtual Project Project { get; set; }

        public virtual User User { get; set; }

        public virtual Resources Quota { get; set; }
        public virtual Resources Resources { get; set; } = Resources.Zero;

        public virtual UseCycleSubject UseCycleSubject { get; set; }

        public ICollection<UseCycleRecord> UseCycleRecords => UseCycleSubject.UseCycleRecords;

        public SubjectType SubjectType => SubjectType.User;

        public UserProjectAssignment(Guid id, User user, Project project,  Resources quota)
        {
            Id = id;
            User = user;
            Project = project;
            Quota = quota;
        }

        public UserProjectAssignment()
        {
        }

        public void Settle(PricePlan plan, DateTime lastSettled, DateTime now)
        {
            UseCycleSubject.Settle(plan, lastSettled, now);
        }
    }
}
