using DAL.Entities.Identity;
using DAL.Identity.Config;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new AddressConfiguration());
            builder.ApplyConfiguration(new ApplicationUserConfiguration());

            builder.Entity<ApplicationUser>().HasMany(e => e.RefreshTokens)
                .WithOne(e => e.ApplicationUser);
        }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
