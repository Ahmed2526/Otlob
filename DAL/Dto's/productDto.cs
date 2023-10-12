namespace DAL.Dto_s
{
    public class productDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string ProductType { get; set; } = null!;
        public string ProductBrand { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
