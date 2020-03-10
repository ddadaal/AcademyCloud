using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
    public class Receiver: IReceiver
    {
        public Guid RecordId { get; set; }
        public virtual User? User { get; set; }

        public virtual Project? Project { get; set; }
        public virtual Domain? Domain { get; set; }

        public virtual System? System { get; set; }

        public SubjectType SubjectType { get; set; }

        private IReceiver RealReceiver => SubjectType switch
        {
            SubjectType.User => User!,
            SubjectType.Project => Project!,
            SubjectType.Domain => Domain!,
            SubjectType.System => System!,
        };

        public Guid Id => RealReceiver.Id;

        public OrgTransaction Receive(IPayer from, User fromUser, decimal amount, TransactionReason reason)
        {
            return RealReceiver.Receive(from, fromUser, amount, reason);
        }

        public Receiver(IReceiver payer)
        {
            switch (payer)
            {
                case User user:
                    User = user;
                    SubjectType = SubjectType.User;
                    break;
                case Project project:
                    Project = project;
                    SubjectType = SubjectType.Project;
                    break;
                case Domain domain:
                    Domain = domain;
                    SubjectType = SubjectType.Domain;
                    break;
                case System system:
                    System = system;
                    SubjectType = SubjectType.System;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(payer));
            }

        }
        protected Receiver()
        {
        }
    }
}
