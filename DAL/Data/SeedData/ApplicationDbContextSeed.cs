using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL.Data.SeedData
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext _context)
        {
            if (!(_context.ProductBrands.Any()))
            {
                var brandsData = File.ReadAllText("../DAL/Data/JsonData/brands.json");

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                foreach (var brand in brands!)
                {
                    await _context.AddAsync(brand);
                }
                await _context.SaveChangesAsync();
            }

            if (!(_context.ProductTypes.Any()))
            {
                var typesData = File.ReadAllText("../DAL/Data/JsonData/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                foreach (var type in types!)
                {
                    await _context.AddAsync(type);
                }
                await _context.SaveChangesAsync();
            }

            if (!(_context.Products.Any()))
            {
                var ProductsData = File.ReadAllText("../DAL/Data/JsonData/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

                foreach (var Product in Products!)
                {
                    await _context.AddAsync(Product);
                }
                await _context.SaveChangesAsync();
            }

        }
    }
}
