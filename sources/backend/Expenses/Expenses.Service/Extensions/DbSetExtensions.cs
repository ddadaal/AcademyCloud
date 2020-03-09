using AcademyCloud.Expenses.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Extensions
{
    public static class DbSetExtensions
    {
        public static async Task<T> FindIfNullThrowAsync<T>(this DbSet<T> dbSet, string id) where T : class
        {
            return await dbSet.FindAsync(Guid.Parse(id)) ?? throw EntityNotFoundException.Create<T>($"ID {id}");
        }

    }
}
