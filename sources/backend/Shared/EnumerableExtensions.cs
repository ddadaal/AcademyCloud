using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyCloud.Shared
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable)
            {
                action(item);
            }
        }

        public static async Task<IEnumerable<TResp>> SelectAsync<TParam, TResp>(this IEnumerable<TParam> enumerable, Func<TParam, Task<TResp>> action)
        {
            return await Task.WhenAll(enumerable.Select(action));
        }

        public static async Task<T2> Then<T1, T2>(this Task<T1> t1, Func<T1, T2> then)
        {
            var a = await t1;
            return then(a);
        }
    }

}
