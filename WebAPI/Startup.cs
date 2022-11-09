using API;
using AutoFilterer.Swagger;
using Business.Config;
using DataAccess;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            string connectionString= Configuration.GetConnectionString("LocalConnection");
 
            services.AddDbContext<SocialMediaContext>(options =>
                options.UseSqlServer(connectionString),
                ServiceLifetime.Transient
            );
            services.AddConfigureDependency();

            services.AddAuthentication();

            services.ConfigureJWT(Configuration);
            services.ConfigureFirebase(Configuration);

            services.AddControllers();
            services.AddHttpContextAccessor();
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

            app.UseSwaggerUI(c =>
            {
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "Hotel Booking API v1");
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


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
