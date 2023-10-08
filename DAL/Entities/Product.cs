using DAL.Entities.baseEntity;

namespace DAL.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public decimal Price { get; set; }

        public int ProductBrandId { get; set; }
        public ProductBrand? ProductBrand { get; set; }

        public int ProductTypeId { get; set; }
        public ProductType? ProductType { get; set; }
    }
}
