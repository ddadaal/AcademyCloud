﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.BackgroundTasks.UseCycle
{
    public class UseCycleConfiguration
    {
        public int CheckCycleMs { get; set; }

        public int SettleCycleMs { get; set; }
    }
}
