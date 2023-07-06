using Ecommerce.Interfaces;
using Ecommerce.Models;
using Ecommerce.Repository;
using Ecommerce.Services;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.Controllers
{
    public class BasketController : Controller
    {
        //private readonly IProductRepository _productRepository;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;
        private readonly IAppUserRepository _appUsers;

        public BasketController(IBasketService basketService, IOrderService orderService, IAppUserRepository appUsers)
        {
            //_productRepository = productRepository;
            _basketService = basketService;
            _orderService = orderService;
            _appUsers = appUsers;
        }

        public ViewResult Index()
        {
            var items = _basketService.GetBasketItems(this.HttpContext);
            /*_basketService.BasketItem = items;

            var BasketVM = new BasketViewModel
            {
                Basket = _basketService,
                BasketTotal = _basketService.GetShoppingBasketTotal()
            };*/

            return View(items);
        }

        [HttpGet]
        public ActionResult AddToBasket(string Id)
        {
            //return await _context.Product.FirstOrDefaultAsync(c => c.Id == id);
            //productId = "1adc0436-7aad-48fc-8706-d3ec33f8200d";
            /*var selectedProduct = await _productRepository.GetByIdAsync(productId);

            if (selectedProduct != null)
            {
                
                _basketService.AddToBasket(this.HttpContext, selectedProduct, 1);
            }*/

            _basketService.AddToBasket(this.HttpContext, Id);

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromBasket(string Id)
        {
            _basketService.RemoveFromBasket(this.HttpContext, Id);

            return RedirectToAction("Index");
        }

        public PartialViewResult BasketSummary()
        {
            var basketSummary = _basketService.GetBasketSummary(this.HttpContext);

            return PartialView(basketSummary);
        }


        [Authorize]
        public async Task<ActionResult> Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //For this next section I need to find out how to pull the user id from logged in session and place it in brackets below
             AppUser customer = await _appUsers.GetByIdAsync(userId);

            if (customer != null)
            {
                Order order = new Order()
                {
                    Email = customer.Email,
                    City = customer.City,
                    //Street = customer.Address.Street,
                    Name = customer.Name,
                    PostCode = customer.PostCode
                };

                return View(order);
            }
            else
            {
                return RedirectToAction("Error");
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(Order order)
        {
            //create a function that allows you to save a users details after they have authenticated

            var basketItems = _basketService.GetBasketItems(this.HttpContext); //difference between this one and brett's is that there is no context being inserted here. Whatever shoppingbasketID is already in context is used?? I think
            order.OrderStatus = "Order Created";
            //order.Email = User.Identity.Name; don't know why this would be needed

            //process payment

            order.OrderStatus = "Payment Processed";
             _orderService.CreateOrder(order, basketItems); //This is currently broken
            _basketService.ClearBasket(this.HttpContext); //same as above, no basket context being inserted

            return RedirectToAction("Thankyou", new { OrderId = order.Id });
        }

        public ActionResult ThankYou(string OrderId)
        {
            ViewBag.OrderId = OrderId;
            return View();
        }
    }
}
