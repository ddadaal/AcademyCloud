using AcademyCloud.Expenses.Domain.Entities.Transaction;
using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemEntity = AcademyCloud.Expenses.Domain.Entities.System;

namespace AcademyCloud.Expenses.Domain.Entities.ManagementFee
{
    public class ManagementFeeEntry
    {
        public Guid Id { get; set; }

        public virtual Payer Payer { get; set; }

        public DateTime LastSettled { get; set; }
        
        public SubjectType SubjectType { get; set; }


        public ManagementFeeEntry(Payer payer)
        {
            Id = payer.Id;
            Payer = payer;
            SubjectType = payer.SubjectType;
            LastSettled = DateTime.UtcNow;
        }

        protected ManagementFeeEntry()
        {

        }
        private TransactionReason Reason => SubjectType switch
        {
            SubjectType.Domain => TransactionReason.DomainManagement,
            SubjectType.Project => TransactionReason.ProjectManagement,
            SubjectType.User => TransactionReason.UserManagement,
            _ => throw new InvalidOperationException(),
        };


        public void Charge(SystemEntity system, decimal amount, DateTime time)
        {
            Payer.Pay(system, amount, Reason, time);
            LastSettled = time;
        }
    }
}
