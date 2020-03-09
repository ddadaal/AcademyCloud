using AcademyCloud.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Domain.ValueObjects
{
    public class Subject: ValueObject
    {
        public SubjectType SubjectType { get; set; }

        public Guid Id { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return SubjectType;
            yield return Id;
        }
    }
} 