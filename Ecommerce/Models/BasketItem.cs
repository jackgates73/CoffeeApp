using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class BasketItem : BaseEntity
    {
        public string BasketId { get; set; }
        public string ProductId { get; set; }
        //public Product? Product { get; set; }
        public int Quantity { get; set; }
        
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }

    }
}
