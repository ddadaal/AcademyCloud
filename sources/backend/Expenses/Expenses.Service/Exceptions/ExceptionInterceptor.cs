using Grpc.Core;
using Grpc.Core.Interceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Expenses.Exceptions
{
    public class ExceptionInterceptor : Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (EntityNotFoundException e)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Can not find entity Type of {e.EntityType} by {e.Predicate}"));
            }
        }
    }
}
