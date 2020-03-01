using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Identity.Extensions
{
    public static class ExceptionExtensions
    {
        public static void ThrowRpcExceptionIfNull<T>(T obj, string? id = null) where T : class
        {
            if (obj == null)
            {
                var message = $"Entity Type of {typeof(T)} of Id {id} is not found.";
                throw new RpcException(new Status(StatusCode.NotFound, message));
            }
        }
    }
}
