using BLL.IRepository;
using BLL.Repositories;
using DAL.Data;
using DAL.Data.SeedData;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Otlob_API.ErrorModel;
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

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //register Services
            builder.Services.registerServices();

            //CORS Policy
            builder.Services.ConfigureCors();

            //invalid model state handling
            builder.Services.ConfigureInvalidModelStateResponse();


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

            app.UseAuthorization();

            app.MapControllers();

            //Seed initial Products.
            await ServiceExtensions.SeedProducts(app);

            app.Run();
        }
    }
}