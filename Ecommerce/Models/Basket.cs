using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class Basket : BaseEntity
    {
        public virtual ICollection<BasketItem> BasketItems { get; set; }
        
        
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }

        public Basket()
        {
            this.BasketItems = new List<BasketItem>();
        }
    }
}
