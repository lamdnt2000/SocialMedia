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
using Business.Repository.PostRepo;
using Business.Repository.ReactionRepo;
using CorePush.Google;
using CorePush.Interfaces;
using Business.ScheduleService;
using Business.Service.WatchlistService;
using Business.Repository.WatchlistRepo;
using Business.Service.WalletService;
using Business.Repository.WalletRepo;
using Business.Repository.TransactionDepositRepo;
using Business.Service.TransactionDepositService;
using Business.Repository.PackageRepo;
using Business.Service.PakageService;
using Business.Service.FeatureService;
using DataAccess.Repository.FeatureRepo;
using DataAccess.Repository.PlanRepo;
using Business.Service.PlanService;
using DataAccess.Repository.FeaturePlanRepo;
using Business.Repository.SubscriptionRepo;
using Business.Service.SubscriptionService;
using DataAccess.Repository.UserTypeRepo;
using Business.Service.DashboardService;
using Business.SignalR;
using Microsoft.AspNetCore.SignalR;
using DataAccess.Repository.NotificationRepo;
using Business.Service.NotificationService;
using Business.Service.HangfireService;

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

            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IWalletRepository, WalletRepository>();

            services.AddScoped<ITransactionDepositRepository, TransactionDepositRepository>();
            services.AddScoped<ITransactionDepositService, TransactionDepositService>();

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


            services.AddScoped<IPostCrawlRepository, PostCrawlRepository>();


            services.AddScoped<IReactionRepository, ReactionRepository>();
            services.AddTransient<IScheduleSocial, ScheduleSocial>();
            services.AddScoped<IFcmSender, FcmSender>();

            services.AddScoped<IWatchlistRepository, WatchlistRepository>();
            services.AddScoped<IWatchlistService, WatchlistService>();

            services.AddScoped<IPackageRepository, PackageRepository>();
            services.AddScoped<IPackageService, PackageService>();

            services.AddScoped<IFeatureRepository, FeatureRepository>();
            services.AddScoped<IFeatureService, FeatureService>();

            services.AddScoped<IPlanRepository, PlanRepository>();
            services.AddScoped<IPlanService, PlanService>();

            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();

            services.AddScoped<IFeaturePlanRepository, FeaturePlanRepository>(); 
            services.AddScoped<IUserTypeRepository, UserTypeRepository>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IHangfireService, HangfireService>();


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

        public static void ConfigureFcm(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<FcmSender>();
            var section = configuration.GetSection("FcmNotification");
            var settings = new FcmSettings();
            section.Bind(settings);
            services.AddSingleton(settings);
        }

        /*public static void ConfigureCache(this IServiceCollection services,  IConfiguration configuration)
        {   
            services.Configure<CacheConfiguration>(configuration.GetSection("CacheConfiguration"));
            //For In-Memory Caching
            services.AddMemoryCache();
            services.AddTransient<MemoryCacheService>();
            services.AddTransient<RedisCacheService>();
            services.AddTransient<Func<CacheTech, ICacheService>>(serviceProvider => key =>
            {
                switch (key)
                {
                    case CacheTech.Memory:
                        return serviceProvider.GetService<MemoryCacheService>();
                    case CacheTech.Redis:
                        return serviceProvider.GetService<RedisCacheService>();
                    default:
                        return serviceProvider.GetService<MemoryCacheService>();
                }
            });
        }*/
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
