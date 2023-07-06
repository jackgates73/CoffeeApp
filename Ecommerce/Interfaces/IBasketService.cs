using Ecommerce.Models;
using Ecommerce.ViewModels;

namespace Ecommerce.Interfaces
{
    public interface IBasketService
    {
        void AddToBasket(HttpContext httpContext, string productId);
        void RemoveFromBasket(HttpContext httpContext, string itemId);
        List<BasketItemViewModel> GetBasketItems(HttpContext httpContext);
        BasketSummaryViewModel GetBasketSummary(HttpContext httpContext);
        void GetBasketTotal();
        void ClearBasket(HttpContext httpContext);

    }
}

