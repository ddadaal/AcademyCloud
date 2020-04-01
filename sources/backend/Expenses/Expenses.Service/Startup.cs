using System;
using AcademyCloud.Expenses.BackgroundTasks;
using AcademyCloud.Expenses.BackgroundTasks.BillingCycle;
using AcademyCloud.Expenses.BackgroundTasks.ManagementFee;
using AcademyCloud.Expenses.BackgroundTasks.UseCycle;
using AcademyCloud.Expenses.Data;
using AcademyCloud.Expenses.Domain.Services.BillingCycle;
using AcademyCloud.Expenses.Domain.Services.ManagementFee;
using AcademyCloud.Expenses.Domain.Services.UseCycle;
using AcademyCloud.Expenses.Exceptions;
using AcademyCloud.Expenses.Extensions;
using AcademyCloud.Expenses.Services;
using AcademyCloud.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace AcademyCloud.Expenses
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc(options =>
            {
                options.Interceptors.Add<ExceptionInterceptor>();
            });
            services.AddDbContext<ExpensesDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("default"), o =>
                {
                    o.EnableRetryOnFailure();
                });
                options.UseLazyLoadingProxies();
            });

            // Strongly typed configuration
            services.AddOptions();

            var managementFeeSection = Configuration.GetSection("ManagementFee");
            services.Configure<BackgroundTasks.ManagementFee.ManagementFeeConfigurations>(managementFeeSection);
            services.Configure<Domain.Services.ManagementFee.ManagementFeeConfigurations>(managementFeeSection);

            var useCycleSection = Configuration.GetSection("UseCycle");
            services.Configure<BackgroundTasks.UseCycle.UseCycleConfigurations>(useCycleSection);
            services.Configure<Domain.Services.UseCycle.UseCycleConfigurations>(useCycleSection);

            var billingCycleSection = Configuration.GetSection("BillingCycle");
            services.Configure<BackgroundTasks.BillingCycle.BillingCycleConfigurations>(billingCycleSection);
            services.Configure<Domain.Services.BillingCycle.BillingCycleConfigurations>(billingCycleSection);

            var jwtSettings = new JwtSettings();
            services.AddSingleton(jwtSettings);

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                 .RequireAuthenticatedUser()
                 .Build();
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


            services.AddHttpContextAccessor();
            services.AddSingleton<TokenClaimsAccessor>();

            services.AddSingleton<ScopedDbProvider>();

            // Prepare database
            services.AddHostedService<DatabaseInitializerHostedService>();

            // Add domain services
            services.AddSingleton<BillingCycleService>();
            services.AddSingleton<UseCycleService>();
            services.AddSingleton<ManagementFeeService>();

            // Add background tasks
            services.AddHostedService<ManagementFeeTask>();
            services.AddHostedService<UseCycleTask>();
            services.AddHostedService<BillingCycleTask>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<BalanceService>();
                endpoints.MapGrpcService<TransactionsService>();
                endpoints.MapGrpcService<BillingService>();
                endpoints.MapGrpcService<InteropService>();
                endpoints.MapGrpcService<IdentityService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
