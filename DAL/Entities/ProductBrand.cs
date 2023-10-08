using DAL.Entities.baseEntity;

namespace DAL.Entities
{
    public class ProductBrand : BaseEntity
    {
        public string Name { get; set; } = null!;

        public IEnumerable<Product>? Products { get; set; }
    }
}
