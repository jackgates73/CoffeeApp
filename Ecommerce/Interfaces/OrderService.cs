using Ecommerce.Models;
using Ecommerce.ViewModels;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;

namespace Ecommerce.Interfaces
{
    public class OrderService : IOrderService
    {
        //private readonly IOrderRepository _orderContext;
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }



        public void CreateOrder(Order baseOrder, List<BasketItemViewModel> basketItems)
        {
            foreach (var item in basketItems)
            {
                baseOrder.OrderItems.Add(new OrderItem()
                {
                    ProductId = item.Id,
                    Image = item.Image,
                    Price = item.Price,
                    ProductName = item.ProductName,
                    Quanity = item.Quantity

                });
            }

            _context.Add(baseOrder);
        }
        public bool Add(Order order)
        {
            _context.Add(order);
            return Save();
        }
        public bool Delete(Order order)
        {
            _context.Remove(order);
            return Save();
        }

        public async Task<Order> GetByIdAsync(string id)
        {
            return await _context.Orders.FirstOrDefaultAsync(c => c.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Order order)
        {
            throw new NotImplementedException();

        }

        public void UpdateOrder(Order updatedOrder)
        {
            _context.Update(updatedOrder);
            _context.SaveChanges();
        }

    

        public async Task<IEnumerable<Order>> GetOrderList()
        {
            return await _context.Orders.ToListAsync();
        }


    }
}
