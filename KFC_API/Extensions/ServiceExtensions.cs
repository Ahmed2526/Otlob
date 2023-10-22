using BLL.IRepository;
using BLL.IService;
using BLL.Repositories;
using BLL.Service;
using DAL.Data;
using DAL.Data.SeedData;
using DAL.Entities.Identity;
using DAL.Identity.SeedData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Otlob_API.ErrorModel;
using System.Text;

namespace Otlob_API.Extensions
{
    public static class ServiceExtensions
    {
        public static void registerServices(this IServiceCollection services)
        {
            // services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSingleton<ILoggerManager, LoggerManager>();

            //Register Services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();

        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew=TimeSpan.FromMinutes(5),
                        ValidIssuer = Configuration["JWT:Issuer"],
                        ValidAudience = Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))
                    };
                });
        }
        public static void ConfigureInvalidModelStateResponse(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.Where(m => m.Value!.Errors.Count > 0)
                                                         .SelectMany(m => m.Value!.Errors)
                                                         .Select(e => e.ErrorMessage).ToArray();

                    var responseMessage = new ApiValidationErrorResponse() { Errors = errors };

                    return new BadRequestObjectResult(responseMessage);
                };
            });

        }

    }
}
