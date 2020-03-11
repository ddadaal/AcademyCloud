﻿using AcademyCloud.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.ValueObjects
{
    public class Resources: ValueObject
    {
        /// <summary>
        /// CPU Core.
        /// </summary>
        public int Cpu { get; set; }
        
        /// <summary>
        /// Memory in GB.
        /// </summary>
        public int Memory { get; set; }

        /// <summary>
        /// Storage in GB
        /// </summary>
        public int Storage { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Cpu;
            yield return Memory;
            yield return Storage;
        }

        public Resources(int cpu, int memory, int storage)
        {
            Cpu = cpu;
            Memory = memory;
            Storage = storage;
        }

        public static Resources operator +(Resources op1, Resources op2)
        {
            return new Resources(op1.Cpu + op2.Cpu, op1.Memory + op2.Memory, op1.Storage + op2.Storage);
        }

        public static Resources Zero => new Resources(0, 0, 0);
    }
}
