using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
    /// <summary>
    /// Dummy entity to persist any type of IPayer into the database
    /// </summary>
    public class Payer : IPayer
    {
        public Guid Id { get; set; }
        public virtual User? User { get; set; }

        public virtual Project? Project { get; set; }
        public virtual Domain? Domain { get; set; }

        public virtual ICollection<OrgTransaction> PayedOrgTransactions { get; set; } 


        /// <summary>
        /// Identify the exact type of Payer
        /// </summary>
        public SubjectType SubjectType { get; set; }

        private IPayer RealPayer => SubjectType switch
        {
            SubjectType.User => User!,
            SubjectType.Project => Project!,
            SubjectType.Domain => Domain!,
            SubjectType.System => throw new InvalidOperationException(),
        };


        public bool Active => RealPayer.Active;

        public bool Pay(IReceiver receiver, decimal amount, TransactionReason reason)
        {
            return RealPayer.Pay(receiver, amount, reason);
        }

        public Payer(IPayer payer)
        {
            Id = payer.Id;
            SubjectType = payer.SubjectType;
            PayedOrgTransactions = new List<OrgTransaction>();
            switch (payer)
            {
                case User user:
                    User = user;
                    break;
                case Project project:
                    Project = project;
                    break;
                case Domain domain:
                    Domain = domain;
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
