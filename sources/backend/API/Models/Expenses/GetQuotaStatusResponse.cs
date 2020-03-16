using AcademyCloud.API.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.Expenses
{
    public class GetQuotaStatusResponse
    {
        public Resources Used { get; set; }
        public Resources Total { get; set; }
    }
}
