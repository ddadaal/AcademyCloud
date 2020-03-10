using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
    /// <summary>
    /// Dummy implementation to persist any type of IPayer into the database
    /// </summary>
    public class Payer : IPayer
    {
        public Guid RecordId { get; set; }
        public virtual User? User { get; set; }

        public virtual Project? Project { get; set; }
        public virtual Domain? Domain { get; set; }

        public SubjectType SubjectType { get; set; }

        private IPayer RealPayer => SubjectType switch
        {
            SubjectType.User => User!,
            SubjectType.Project => Project!,
            SubjectType.Domain => Domain!,
            SubjectType.System => throw new InvalidOperationException(),
        };

        public Guid Id => RealPayer.Id;

        public bool Active => RealPayer.Active;

        public bool Pay(IReceiver receiver, decimal amount, TransactionReason reason)
        {
            return RealPayer.Pay(receiver, amount, reason);
        }

        public Payer(IPayer payer)
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(payer));
            }

        }
        protected Payer()
        {
        }
    }
}
