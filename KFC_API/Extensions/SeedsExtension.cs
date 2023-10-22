using DAL.Data.SeedData;
using DAL.Data;
using DAL.Entities.Identity;
using DAL.Identity.SeedData;
using Microsoft.AspNetCore.Identity;

namespace Otlob_API.Extensions
{
    public class SeedsExtension
    {
        public static async Task SeedProducts(WebApplication app)
        {
            var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var AppDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await ApplicationDbContextSeed.SeedAsync(AppDbContext);
        }
        public static async Task SeedUsers(WebApplication app)
        {
            var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await SeedAdmins.SeedAsync(userManager);
        }
    }
}
