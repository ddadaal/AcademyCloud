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

        public virtual ICollection<UseCycle> UseCycleRecords { get; set; } = new List<UseCycle>();

        public void Settle(Resources resources, decimal price)
        {
            throw new NotImplementedException();
        }

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
    }
}
