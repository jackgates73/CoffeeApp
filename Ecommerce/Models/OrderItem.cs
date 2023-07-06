namespace Ecommerce.Models
{
    public class OrderItem : BaseEntity
    {
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Quanity { get; set; } //spelling error, need to recreate database table if I want to fix
    }
}
