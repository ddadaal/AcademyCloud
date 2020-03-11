using AcademyCloud.Expenses.Domain.Entities;
using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemEntity = AcademyCloud.Expenses.Domain.Entities.System;

namespace AcademyCloud.Expenses.BackgroundTasks.ManagementFee
{
    public class ManagementFeeEntry
    {
        public Guid Id { get; set; }

        public virtual Payer Payer { get; set; }

        public DateTime LastSettled { get; set; }

        public decimal Amount { get; set; }

        public ManagementFeeEntry(Guid id, Payer payer, decimal amount)
        {
            Id = id;
            Payer = payer;
            Amount = amount;
            LastSettled = DateTime.UtcNow;
        }

        protected ManagementFeeEntry()
        {

        }
        private TransactionReason Reason => Payer.SubjectType switch
        {
            SubjectType.Domain => TransactionReason.DomainManagement,
            SubjectType.Project => TransactionReason.ProjectManagement,
            SubjectType.User => TransactionReason.UserManagement,
            SubjectType.System => throw new InvalidOperationException(),
        };


        public void Charge(SystemEntity system, DateTime time)
        {
            Payer.Pay(system, Amount, Reason);
            LastSettled = time;
        }
    }
}
