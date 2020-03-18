using AcademyCloud.Expenses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.Entities
{
    /// <summary>
    /// Price plan.
    /// Currently it's just a singleton.
    /// </summary>
    public class PricePlan
    {
        /// <summary>
        /// CPU Core dollar per hour
        /// </summary>
        public decimal Cpu { get; private set; }

        /// <summary>
        /// Memory dollar per MB
        /// </summary>
        public decimal Memory { get; private set; }

        /// <summary>
        /// Storage dollar per GB
        /// </summary>
        public decimal Storage { get; private set; }

        public decimal Calculate(Resources resources)
        {
            return resources.Cpu * Cpu + resources.Memory * Memory + resources.Storage * Storage;
        }

        private PricePlan() { }

        /// <summary>
        /// Sample price plan.
        /// 1 Core, 2 GB RAM (2048 MB Memory) and 40 GB Storage causes 50.7 per month.
        /// </summary>
        public static PricePlan Instance => new PricePlan { Cpu = 0.01m, Memory = 0.00001m, Storage = 0.001m };
    }
}
