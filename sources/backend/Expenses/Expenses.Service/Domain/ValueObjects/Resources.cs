using AcademyCloud.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.ValueObjects
{
    public class Resources: ValueObject
    {
        public int Cpu { get; set; }
        
        public int Memory { get; set; }

        public int Storage { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Cpu;
            yield return Memory;
            yield return Storage;
        }
    }
}
