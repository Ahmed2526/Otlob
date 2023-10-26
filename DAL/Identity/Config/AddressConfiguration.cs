using DAL.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Identity.Config
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(e => e.Country).IsRequired().HasMaxLength(100);
            builder.Property(e => e.City).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Street).IsRequired().HasMaxLength(200);
            builder.Property(e => e.ZipCode).IsRequired().HasMaxLength(15);
        }
    }
}
