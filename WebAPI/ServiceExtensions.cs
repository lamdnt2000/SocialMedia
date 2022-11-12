using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.OpenApi.Models;
using Business.Repository.RoleRepo;
using DataAccess.Models;
using Business.Service.Rule;
using Microsoft.Extensions.Configuration;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Business.Repository.UserRepo;
using Business.Service.UserService;
using API.Service.Authorize;
using Business.Repository.OrganizationRepo;
using Business.Service.OrganizationService;
using Business.Repository.BrandRepo;
using Business.Service.BrandService;
using Business.Repository.PlatformRepo;
using Business.Service.PlatformService;
using Business.Service.CategoryService;
using Business.Repository.CategoryRepo;
using Business.Repository.LocationRepo;
using Business.Service.LocationService;
using Business.Repository.ReactionTypeRepo;
using Business.Service.ReactionTypeService;
using Business.Repository.ChannelCrawlRepo;
using Business.Service.ChannelCrawlService;
using Business.Repository.ChannelRecordRepo;
using Business.Service.ChannelRecordService;
using Business.Repository.PostRepo;
using Business.Service.PostService;
using Business.Repository.ReactionRepo;
using Business.Service.ReactionService;

namespace API
{
    public static class ServiceExtensions
    {
        public static void AddConfigureDependency(this IServiceCollection services)
        {
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IOrganizationService, OrganizationService>();

            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IBrandService, BrandService>();

            services.AddScoped<IPlatformRepository, PlatformRepository>();
            services.AddScoped<IPlatformService, PlatformService>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ILocationService, LocationService>();

            services.AddScoped<IReactionTypeRepository, ReactionTypeRepository>();
            services.AddScoped<IReactionTypeService, ReactionTypeService>();

            services.AddScoped<IChannelCrawlRepository, ChannelCrawlRepository>();
            services.AddScoped<IChannelCrawlService, ChannelCrawlService>();

            services.AddScoped<IChannelRecordRepository, ChannelRecordRepository>();
            services.AddScoped<IChannelRecordService, ChannelRecordService>();

            services.AddScoped<IPostCrawlRepository, PostCrawlRepository>();
            services.AddScoped<IPostCrawlService, PostCrawlService>();

            services.AddScoped<IReactionRepository, ReactionRepository>();
            services.AddScoped<IReactionService, ReactionService>();


        }


        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        Log.Error($"Something went wrong in the {contextFeature.Error}");

                        await context.Response.WriteAsync(new Error
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error. Please Try Again Later."
                        }.ToString());
                    }
                });
            });
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var key = configuration["Jwt:Secret"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                };
            });
        }

        public static void ConfigureFirebase(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FirebaseMetadata>(options => configuration.GetSection("Firebase").Bind(options));
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(configuration["FirebasePrivateKey"])
            });
        }


        public static void ConfigureCros(this IServiceCollection services)
        {
            services.AddCors(cors =>
            {
                cors.AddPolicy("AllowAll", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "SocialMedia.API", Version = "v1" });
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }
    }
}
