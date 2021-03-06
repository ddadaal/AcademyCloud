﻿using AcademyCloud.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.ValueObjects
{
    public class TransactionReason : ValueObject
    {
        public TransactionType Type { get; set; }

        public string Info { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Type;
            yield return Info;
        }

        public TransactionReason(TransactionType type, string info)
        {
            Type = type;
            Info = info;
        }

        public static TransactionReason Charge => new TransactionReason(TransactionType.Charge, "");
        public static TransactionReason DomainManagement => new TransactionReason(TransactionType.DomainManagementFee, "");
        public static TransactionReason ProjectManagement => new TransactionReason(TransactionType.ProjectManagementFee, "");
        public static TransactionReason UserManagement => new TransactionReason(TransactionType.UserManagementFee, "");
        public static TransactionReason DomainQuota => new TransactionReason(TransactionType.DomainQuota, "");
        public static TransactionReason DomainQuotaChange => new TransactionReason(TransactionType.DomainQuotaChange, "");
        public static TransactionReason ProjectQuota => new TransactionReason(TransactionType.ProjectQuota, "");
        public static TransactionReason ProjectQuotaChange => new TransactionReason(TransactionType.ProjectQuotaChange, "");
        public static TransactionReason SocialResourcesChange => new TransactionReason(TransactionType.SocialResourcesChange, "");
        public static TransactionReason UserProjectQuota => new TransactionReason(TransactionType.UserProjectQuota, "");
    }
}
