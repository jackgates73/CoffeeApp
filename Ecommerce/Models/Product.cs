using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Product : BaseEntity
    {

        public string? ProfileImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }
}
