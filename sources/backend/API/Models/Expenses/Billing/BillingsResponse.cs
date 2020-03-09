using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.Expenses.Billing
{
    public class BillingsResponse<T>
    {
        public IEnumerable<T> Billings { get; set; }

    }
}
