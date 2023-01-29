using API;
using Business.Config;
using Business.ScheduleService;
using Business.Service.NotificationService;
using Business.Service.ScheduleService;
using Business.SignalR;
using DataAccess;
using DataAccess.Models.ConfigModel;
using DataAccess.Repository.NotificationRepo;
using Hangfire;
using Hangfire.Console;
using Hangfire.SqlServer;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using WebAPI.Config;

namespace WebAPI
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("LocalConnection");
            services.AddDbContext<SocialMediaContext>(options =>
                options.UseSqlServer(connectionString),
                ServiceLifetime.Transient
            );

            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            var hangfireConnectionString = Configuration.GetConnectionString("HangfireConnection");
            services.AddHangfire((provider, configuration) =>
            {
                configuration
           .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
               .UseSimpleAssemblyNameTypeSerializer()
               .UseRecommendedSerializerSettings()
               .UseSqlServerStorage(hangfireConnectionString, new SqlServerStorageOptions
               {
                   CommandBatchMaxTimeout = TimeSpan.FromHours(1),
                   SlidingInvisibilityTimeout = TimeSpan.FromHours(1),
                   QueuePollInterval = TimeSpan.Zero,
                   UseRecommendedIsolationLevel = true,
                   DisableGlobalLocks = true
               });
                configuration.UseConsole();
                configuration.UseFilter(new JobStatusFilter(
                    services.BuildServiceProvider().GetRequiredService<IScheduleSocial>()
                    ));

            });
            var queueSettings = Configuration.GetSection("Worker").Get<List<HangfireQueueSetting>>();
            foreach (var setting in queueSettings)
            {
                services.AddHangfireServer(options =>
                {
                    options.ServerName = $"{Environment.MachineName}:{setting.QueueName}";
                    options.Queues = new[] { setting.QueueName };
                    options.WorkerCount = setting.WorkerCount;
                });
            }
            services.AddConfigureDependency();
            services.AddDistributedMemoryCache();
            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = Configuration.GetConnectionString("Caching");
                options.SchemaName = "dbo";
                options.TableName = "Cache";
            });
            services.Configure<PaymentConfig>(Configuration.GetSection("Payment"));
            services.AddAuthentication();
            services.AddSignalR()
            .AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                options.PayloadSerializerOptions.Encoder = null;
                options.PayloadSerializerOptions.IncludeFields = false;
                options.PayloadSerializerOptions.IgnoreReadOnlyFields = false;
                options.PayloadSerializerOptions.IgnoreReadOnlyProperties = false;
                options.PayloadSerializerOptions.MaxDepth = 0;
                options.PayloadSerializerOptions.NumberHandling = JsonNumberHandling.Strict;
                options.PayloadSerializerOptions.DictionaryKeyPolicy = null;
                options.PayloadSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
                options.PayloadSerializerOptions.PropertyNameCaseInsensitive = false;
                options.PayloadSerializerOptions.DefaultBufferSize = 32_768;
                options.PayloadSerializerOptions.ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip;
                options.PayloadSerializerOptions.ReferenceHandler = null;
                options.PayloadSerializerOptions.WriteIndented = true;

            });
            services.ConfigureJWT(Configuration);
            services.ConfigureFirebase(Configuration);
            services.AddHangfireServer(option =>
            {
                option.SchedulePollingInterval = TimeSpan.FromSeconds(1);
            });
            services.AddControllers();

            services.AddHttpContextAccessor();
            services.ConfigureFcm(Configuration);
            services.ConfigureCros();
            services.ConfigureSwagger();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                    c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "Social Media API v1");
                });
            }

            app.UseSwagger();
            app.UseStaticFiles();
            app.UseSwaggerUI(c =>
            {
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "Social Media API v1");
            });

            app.ConfigureExceptionHandler();


            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseRouting();

            //configure auth
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                //AppPath = "" //The path for the Back To Site link. Set to null in order to hide the Back To  Site link.
                DashboardTitle = "My Website",
                Authorization = new[]
                {
                    new HangfireCustomBasicAuthenticationFilter{
                        User = Configuration.GetSection("HangfireSettings:UserName").Value,
                        Pass = Configuration.GetSection("HangfireSettings:Password").Value
                    }
                }


            });
            //custom jwt auth middleware
            app.UseMiddleware<JWTMiddlewareConfig>();

            app.UseMiddleware<RateLimitingMiddleware>();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/notification", options =>
                {
                    options.Transports =
                HttpTransportType.WebSockets |
                HttpTransportType.LongPolling;
                    options.ApplicationMaxBufferSize = 65_536;
                    options.TransportMaxBufferSize = 65_536;
                    options.MinimumProtocolVersion = 0;
                    options.WebSockets.CloseTimeout = TimeSpan.FromSeconds(5);
                    options.LongPolling.PollTimeout = TimeSpan.FromSeconds(10);
                    Console
                    .WriteLine($"Authorization data items: {options.AuthorizationData.Count}");
                });
            });

        }
    }
}
