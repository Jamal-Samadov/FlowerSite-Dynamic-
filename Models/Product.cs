namespace FlowerSite.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public int? Discount { get; set; }
        public string? Description { get; set; }
        public int Available { get; set; }
        public string? SKU { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? ImageUrl { get; set; }
    }
}
