using API;
using Business.Config;
using Business.SignalR;
using DataAccess;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json.Serialization;

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
            var hangfireConnectionString = Configuration.GetConnectionString("HangfireConnection");
            services.AddHangfire(configuration => configuration
           .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
               .UseSimpleAssemblyNameTypeSerializer()
               .UseRecommendedSerializerSettings()
               .UseSqlServerStorage(hangfireConnectionString, new SqlServerStorageOptions
               {
                   CommandBatchMaxTimeout = TimeSpan.FromMinutes(30),
                   SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                   QueuePollInterval = TimeSpan.Zero,
                   UseRecommendedIsolationLevel = true,
                   DisableGlobalLocks = true
               }));

            services.AddConfigureDependency();

           /* services.AddCors(options =>
            {
                options.AddPolicy(name: "localhost",
                                  builder =>
                                  {
                                      builder.WithOrigins("https://localhost:44335/");
                                  });
            });*/
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

            //custom jwt auth middleware
            app.UseMiddleware<JWTMiddlewareConfig>();
            app.UseHangfireDashboard();
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
                    options.WebSockets.CloseTimeout = TimeSpan.FromSeconds(3);
                    options.LongPolling.PollTimeout = TimeSpan.FromSeconds(10);
                    Console
                    .WriteLine($"Authorization data items: {options.AuthorizationData.Count}");
                });
            });

        }
    }
}
