using DAL.Entities.baseEntity;

namespace DAL.Entities
{
    public class ProductType : BaseEntity
    {
        public string Name { get; set; } = null!;

        public IEnumerable<Product>? Products { get; set; }

    }
}
