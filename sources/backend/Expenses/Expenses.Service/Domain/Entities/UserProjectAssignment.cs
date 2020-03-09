using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
    public class UserProjectAssignment
    {

        public Guid Id { get; set; }
        public virtual Project Project { get; set; }

        public virtual User User { get; set; }

        public virtual Resources Quota { get; set; }

        public virtual ICollection<UseCycle> UseCycleRecords { get; set; }

    }
}
