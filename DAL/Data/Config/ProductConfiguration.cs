using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(e => e.Name).IsRequired().HasMaxLength(250);
            builder.Property(e => e.PictureUrl).IsRequired().HasMaxLength(500);
            builder.Property(e => e.Description).IsRequired().HasMaxLength(2000);
        }
    }
}
