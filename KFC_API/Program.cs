using DAL.Data;
using DAL.Entities.Identity;
using DAL.Identity;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using Otlob_API.Extensions;
using Otlob_API.Middlewares;

namespace KFC_API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //Configure Nlog
            LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(),
                   "/nlog.config"));

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            //Identity
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDbConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<AppIdentityDbContext>();


            //Configure Response format
            builder.Services.AddControllers(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
            }).AddXmlDataContractSerializerFormatters()
            .AddCustomCSVFormatter();



            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Configure jwt
            builder.Services.ConfigureJWT(builder.Configuration);

            //register Services
            builder.Services.registerServices();

            //Configure Redis
            builder.ConfigureRedis();

            //CORS Policy
            builder.Services.ConfigureCors();

            //invalid model state handling
            builder.Services.ConfigureInvalidModelStateResponse();

            //Mapping from appsettings to class
            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                //   app.UseDeveloperExceptionPage();
            }
            else
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            //Seed initial Products.
            await SeedsExtension.SeedProducts(app);
            await SeedsExtension.SeedUsers(app);

            app.Run();
        }
    }
}