using DAL.Entities.baseEntity;

namespace DAL.Entities.Identity
{
    public class Address : BaseEntity
    {
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
    }
}
