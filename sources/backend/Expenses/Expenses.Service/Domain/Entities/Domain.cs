﻿using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
    public class Domain: IPayer, IReceiver
    {
        public Guid Id { get; set; }

        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

        public virtual ICollection<UserDomainAssignment> Users { get; set; } = new List<UserDomainAssignment>();

        public virtual User PayUser { get; set; }

        public virtual Resources Quota { get; set; }
        
        public virtual Payer Payer { get; set; }
        public virtual Receiver Receiver { get; set; }


        public virtual ICollection<UseCycle> UseCycleRecords { get; set; } = new List<UseCycle>();

        public virtual ICollection<BillingCycle> BillingCycleRecords { get; set; } = new List<BillingCycle>();

        public bool Active => PayUser.Active;


        public ICollection<OrgTransaction> PayedOrgTransactions => Payer.PayedOrgTransactions;
        public ICollection<OrgTransaction> ReceivedOrgTransactions => Receiver.ReceivedOrgTransactions;

        public SubjectType SubjectType => SubjectType.Domain;

        public Resources Resources => Projects.Select(x => x.Resources).Aggregate(Resources.Zero, (s, a) => s + a);

        public User ReceiveUser => PayUser;

        public bool Pay(IReceiver receiver, decimal amount, TransactionReason reason)
        {
            return Payer.Pay(receiver, amount, reason);
        }

        public OrgTransaction Receive(IPayer from, User fromUser, decimal amount, TransactionReason reason)
        {
            return Receiver.Receive(from, fromUser, amount, reason);
        }

        public Domain(Guid id, User payer, Resources quota)
        {
            Id = id;
            PayUser = payer;
            Quota = quota;

            Payer = new Payer(this);
            Receiver = new Receiver(this);
        }

        public Domain()
        {
        }
    }
}
