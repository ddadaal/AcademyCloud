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

        public EntityNotFoundException(Type entityType, string predicate)
        {
            EntityType = entityType;
            Predicate = predicate;
        }

        public static EntityNotFoundException Create<T>(string predicate)
        {
            return new EntityNotFoundException(typeof(T), predicate);
        }
    }
}
