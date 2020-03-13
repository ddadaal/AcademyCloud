using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.Common
{
    public class Resources
    {
        public int Cpu { get; set; }
        public int Memory { get; set; }

        public int Storage { get; set; }

        public static implicit operator Resources(AcademyCloud.Expenses.Protos.Common.Resources resources)
        {
            return new Resources() { Cpu = resources.Cpu, Memory = resources.Memory, Storage = resources.Storage };
        }

        public static implicit operator AcademyCloud.Expenses.Protos.Common.Resources(Resources resources)
        {
            return new AcademyCloud.Expenses.Protos.Common.Resources() { Cpu = resources.Cpu, Memory = resources.Memory, Storage = resources.Storage };
        }
    }
}
