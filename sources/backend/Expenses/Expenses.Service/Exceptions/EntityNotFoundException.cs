using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Exceptions
{
    public class EntityNotFoundException: Exception
    {
        public Type EntityType { get; set; }
        public string Predicate { get; set; }

        public EntityNotFoundException(Type entityType, params object[] predicates)
        {
            EntityType = entityType;
            Predicate = string.Join(", ", predicates);
        }

        public static EntityNotFoundException Create<T>(params object[] predicates)
        {
            return new EntityNotFoundException(typeof(T), predicates);
        }
    }
}
