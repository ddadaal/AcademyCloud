using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.ValueObjects
{
    public enum TransactionType
    {
        Charge,
        UserManagementFee,
        DomainManagementFee,
        ProjectManagementFee,
        DomainResources,
        ProjectResources,
        DomainQuotaChange,
        ProjectQuotaChange,
    }
}
