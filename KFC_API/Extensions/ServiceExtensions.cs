using BLL.IRepository;
using BLL.IService;
using BLL.Repositories;
using BLL.Service;
using DAL.Data;
using DAL.Data.SeedData;
using Microsoft.AspNetCore.Mvc;
using Otlob_API.ErrorModel;

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

        public static async Task SeedProducts(WebApplication app)
        {
            var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var AppDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await ApplicationDbContextSeed.SeedAsync(AppDbContext);
        }
    }
}
