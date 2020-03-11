﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.BackgroundTasks.ManagementFee
{
    public class ManagementFeeConfiguration
    {
        public int CheckCycleMs { get; set; }
        public int ChargeCycleMs { get; set; }
    }
}
