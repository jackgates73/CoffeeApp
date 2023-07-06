using Ecommerce.Models;
using Ecommerce.ViewModels;

namespace Ecommerce.Interfaces
{
    public interface IOrderService
    {
        void CreateOrder(Order baseOrder, List<BasketItemViewModel> basketItems);

        Task<Order> GetByIdAsync(string id);

        Task<IEnumerable<Order>> GetOrderList();

        void UpdateOrder(Order updatedOrder);
        bool Add(Order order);
        bool Update(Order order);
        bool Delete(Order order);
        bool Save();
    }
}
