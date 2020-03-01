using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Utils
{
    public class AuthenticatedCallInterceptor : Interceptor
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthenticatedCallInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var token = httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (token != StringValues.Empty)
            {
                context.Options.Headers.Add("Authorization", token);
            }
            return continuation(request, context);
        }
    }
}
