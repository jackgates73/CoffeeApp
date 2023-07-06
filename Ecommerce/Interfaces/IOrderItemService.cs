using Ecommerce.Models;

namespace Ecommerce.Interfaces
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItem>> GetOrderItemList(string orderId);
    }
}
