using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;

namespace Ecommerce.Interfaces
{
    public class OrderItemService : IOrderItemService
    {

        //private readonly IOrderRepository _orderContext;
        private readonly ApplicationDbContext _context;

        public OrderItemService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<OrderItem>> GetOrderItemList(string orderId)
        {
            return await _context.OrderItems.Where(c => c.OrderId == orderId).ToListAsync();
        }
    }
}
