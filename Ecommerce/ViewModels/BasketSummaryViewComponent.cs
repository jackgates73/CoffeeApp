using Ecommerce.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.ViewModels
{
    public class BasketSummaryViewComponent : ViewComponent
    {
        private readonly IBasketService _basketService;

        public BasketSummaryViewComponent(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var basketSummary = _basketService.GetBasketSummary(HttpContext);

            return View(basketSummary);
        }
    }
}
