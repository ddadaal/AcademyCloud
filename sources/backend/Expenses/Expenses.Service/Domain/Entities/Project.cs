using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
    public class Project: IPayer, IReceiver
    {
        public Guid Id { get; set; }

        public virtual ICollection<UserProjectAssignment> Users { get; set; }

        public virtual User Payer { get; set; }
        public virtual Domain Domain { get; set; }
        
        public virtual Resources Quota { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<UseCycle> UseCycleRecords { get; set; }

        public virtual ICollection<BillingCycle> BillingCycleRecords { get; set; }

        public SubjectType SubjectType => SubjectType.Project;

        public bool Pay(IReceiver receiver, decimal amount, TransactionReason reason)
        {
            throw new NotImplementedException();
        }
    }
}
