using Ecommerce.Interfaces;
using Ecommerce.Models;
using Ecommerce.Repository;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Ecommerce.Services
{
    public class BasketService : IBasketService
    {
        IRepository<Basket> _basketContext;
        IRepository<BasketItem> _basketItemContext;
        IRepository<Product> _productContext;
        private readonly UserManager<AppUser> _userManager;


        private readonly ApplicationDbContext _appDbContext;

        public BasketService(IRepository<Product> productRepository, IRepository<Basket> basketContext, ApplicationDbContext appDbContext, IRepository<BasketItem> basketItemContext, UserManager<AppUser> userManager)
        {
            _productContext = productRepository;
            _basketContext = basketContext;
            _appDbContext = appDbContext;
            _basketItemContext = basketItemContext;
            _userManager = userManager;
        }

        public const string BasketSessionName = "eCommerceBasket";

        private Basket GetBasket(HttpContext httpContext, bool createIfNull)
        {
            //read cookie from IHttpContextAccessor  
            var cookie = httpContext.Request.Cookies[BasketSessionName];

            Basket basket = new Basket();

            
            if (httpContext.User.Identity.IsAuthenticated == false)
            {
                basket = _appDbContext.Baskets.Include(b => b.BasketItems).FirstOrDefault(b => b.Id == cookie);
                if (cookie != null)
                {
                    string basketId = cookie;
                    if (!string.IsNullOrEmpty(basketId))
                    {
                        //Get basket is broken. No idea but it returns 0 basketitems from the DB. One time it returned the items somehow (couldn't recreate). AddToBasket gives this 1 basketitem and this is saved to the basketcontext, so no idea why this is reset here
                        //I fixed this by adding Include!!
                        basket = _appDbContext.Baskets.Include(b => b.BasketItems).FirstOrDefault(b => b.Id == basketId);
                    }
                    else
                    {
                        if (createIfNull)
                        {
                            basket = CreateNewBasket(httpContext);
                        }
                    }
                }
                else
                {
                    if (createIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }
            }
            else
            {
                string currentUserId = _userManager.GetUserId(httpContext.User);
                basket = _appDbContext.Baskets.Include(b => b.BasketItems).FirstOrDefault(b => b.AppUserId == currentUserId);
                //basket.AppUserId = currentUserId;

                if (basket == null)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }

            return basket;

        }

        private Basket CreateNewBasket(HttpContext httpContext)
        {
            Basket basket = new Basket();


            if (httpContext.User.Identity.IsAuthenticated == false)
            {
                var cookie = new CookieOptions();
                cookie.Expires = DateTime.Now.AddDays(1);
                cookie.Path = "/";
                cookie.Secure = true; // Set secure flag
                cookie.HttpOnly = true; // Set httpOnly flag
                httpContext.Response.Cookies.Append(BasketSessionName, basket.Id, cookie);
                basket.AppUserId = Guid.NewGuid().ToString();
            }
            else
            {
                //storing users ID in Basket
                string currentUserId = _userManager.GetUserId(httpContext.User);
                basket.AppUserId = currentUserId;
            } 
            
            _basketContext.Insert(basket);
            _basketContext.Commit();


            return basket;
        }



        public void AddToBasket(HttpContext httpContext, string productId)
        {
            Basket basket =  GetBasket(httpContext, true);
            string currentUserId = _userManager.GetUserId(httpContext.User);
            if (currentUserId == null)
            {
                currentUserId = Guid.NewGuid().ToString();  
            }
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);

            //item = null even though a basketItem with the same productID and basketId exists in DB
            //item is null because getbasket returns BasketItems with 0 items for some reason. GetBasket is broken
            //there is no basketItems column in basket DB, is this expected?
            //or, DB looks like everything adds up, maybe I'm just having issues displaying basket

            //OKAY, so basket is returning everything except the basketitems. What I need to do is:
            //string basketIDvar = basket.ID
            //pull basketItems into the context. Get BasketItems, where basketIDvar = BasketItem.BasketID

            //Problem Resolved!!! GThe whole issue with basketItems not being pulled from the DB, was because I wasn't using an 'Include' method for this. I had to use appdbcontext instead to include the basketitems model within the basket model

            if (item == null)
            {
                item = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1,
                    AppUserId = currentUserId
                };

                basket.BasketItems.Add(item);
            }
            else
            {
                item.Quantity = item.Quantity + 1;
            }
            _basketContext.Commit();
        }

        public void RemoveFromBasket(HttpContext httpContext, string itemId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);

            if (item != null)
            {
                basket.BasketItems.Remove(item);
                _basketContext.Commit();
            }
        }

       

        public List<BasketItemViewModel> GetBasketItems(HttpContext httpContext)
        {
            Basket basket = GetBasket(httpContext, false);

            if (basket != null)
            {
                var results = (from b in basket.BasketItems
                               join p in _productContext.Collection() on b.ProductId equals p.Id
                               select new BasketItemViewModel()
                               {
                                   Id = b.Id,
                                   Quantity = b.Quantity,
                                   ProductName = p.Title,
                                   Image = p.ProfileImageUrl,
                                   Price = p.Price
                               }
                              ).ToList();

                return results;
            }
            else
            {
                return new List<BasketItemViewModel>();
            }
        }
        //follow other tutorial
       public BasketSummaryViewModel GetBasketSummary(HttpContext httpContext)
        {
              Basket basket = GetBasket(httpContext, false);
              BasketSummaryViewModel model = new BasketSummaryViewModel(0, 0);
              if (basket != null)
              {
                  int? basketCount = (from item in basket.BasketItems
                                      select item.Quantity).Sum();

                  decimal? basketTotal = (from item in basket.BasketItems
                                          join p in _productContext.Collection() on item.ProductId equals p.Id
                                          select item.Quantity * p.Price).Sum();

                  model.BasketCount = basketCount ?? 0;
                  model.BasketTotal = basketTotal ?? decimal.Zero;

                  return model;
              }
              else
              {
                  return model;
              }
          
            throw new NotImplementedException();
        } 


        public void ClearBasket(HttpContext httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            basket.BasketItems.Clear();
            _basketContext.Commit();
        }

        public void GetBasketTotal()
        {
            throw new NotImplementedException();
        }
    }
}
