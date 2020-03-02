using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Exceptions;
using AcademyCloud.Identity.Services;
using AcademyCloud.Identity.Services.Account;
using AcademyCloud.Identity.Services.Authentication;
using AcademyCloud.Identity.Services.Domains;
using AcademyCloud.Identity.Services.Projects;
using AcademyCloud.Identity.Services.Users;
using AcademyCloud.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace AcademyCloud.Identity
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc(options =>
            {
                options.Interceptors.Add<ExceptionInterceptor>();
            });
            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseInMemoryDatabase("Test");
            });

            var jwtSettings = new JwtSettings();
            services.AddSingleton(jwtSettings);

            services.AddAuthorization(options =>
            {
                var defaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();

                options.DefaultPolicy = defaultPolicy;

                options.AddPolicy(AuthPolicy.System,
                    policy => policy.Combine(defaultPolicy).RequireAssertion(c => c.User.ToTokenClaims().IsSystem));

            });

            services
                .AddAuthentication(config =>
                {
                    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(config =>
                {
                    config.RequireHttpsMetadata = false;
                    config.SaveToken = true;
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Issuer,
                        IssuerSigningKey = jwtSettings.Key,
                        RequireExpirationTime = false,
                        ClockSkew = TimeSpan.Zero,
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IdentityDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // temp: to generate data into the test db
            dbContext.Database.EnsureCreated();


            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<AccountService>();
                endpoints.MapGrpcService<AuthenticationService>();
                endpoints.MapGrpcService<DomainsService>();
                endpoints.MapGrpcService<ProjectsService>();
                endpoints.MapGrpcService<UsersService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
