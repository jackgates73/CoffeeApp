using Ecommerce.Interfaces;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Authorize(Roles = "admin")]
    public class OrderManagerController : Controller
    {
        
            IOrderService _orderService;
            IOrderItemService _orderItemService;

            public OrderManagerController(IOrderService OrderService, IOrderItemService OrderItemService)
            {
                _orderService = OrderService;
                _orderItemService = OrderItemService;
            }

            // GET: OrderManager
            public async Task<ActionResult> Index()
            {
                List<Order> orders = (List<Order>)await _orderService.GetOrderList();

                return View(orders);
            }

            public async Task<ActionResult> UpdateOrder(string Id)
            {
                ViewBag.StatusList = new List<string>() {
                "Order Created",
                "Payment Processed",
                "Order Shipped",
                "Order Complete"
            };
                Order order = await _orderService.GetByIdAsync(Id);

                IEnumerable<OrderItem> orderItems = await _orderItemService.GetOrderItemList(order.Id);
                ViewData["OrderItems"] = orderItems;

                return View(order);
            }

            [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateOrder(Order updatedOrder, string Id)
            {
                Order order = await _orderService.GetByIdAsync(Id);

                order.OrderStatus = updatedOrder.OrderStatus;
                _orderService.UpdateOrder(order);

                return RedirectToAction("Index");
            }
        }
    
}
