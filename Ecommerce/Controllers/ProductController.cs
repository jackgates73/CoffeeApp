using Ecommerce.Interfaces;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Data;

namespace Ecommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;


        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string productFilter=null)
        {
            if (productFilter == null)
            {
                var products = await _productRepository.GetAll();
                return View(products);
            }
            else
            {
                var products = await _productRepository.GetFilterList(productFilter);
                return View(products);
            }
        }
        

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            Product product = await _productRepository.GetByIdAsync(id);
            return View(product);
        }

        //[HttpPost]
        //public async Task<IActionResult> AddToBasket(string id)
        //{
        //    Product product = await _productRepository.GetByIdAsync(id);

        //    BasketService.AddToBasket(product, 1);
        //    return View();
        //}

        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            Product product = new Product();
            return View(product);
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                _productRepository.Add(product);
                return RedirectToAction("Index");
            }

        }


        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(string Id)
        {
            Product productToDelete = await _productRepository.GetByIdAsync(Id);

            if (productToDelete == null)
            {
                throw new NotImplementedException();
            }
            else
            {
                return View(productToDelete);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmDelete(string Id)
        {
            Product productToDelete = await _productRepository.GetByIdAsync(Id);

            if (productToDelete == null)
            {
                throw new NotImplementedException();
            }
            else
            {
                          
                _productRepository.Delete(productToDelete);
                return RedirectToAction("Index");
            }
        }



        public async Task<ActionResult> Edit(string Id)
        {
            Product product = await _productRepository.GetByIdAsync(Id);
            if (product == null)
            {
                throw new NotImplementedException();
            }
            else
            {
                return View(product);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Product product, string Id)
        {
            Product productToEdit = await _productRepository.GetByIdAsync(Id);

            if (productToEdit == null)
            {
                throw new NotImplementedException();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }

                
                productToEdit.Description = product.Description;
                productToEdit.Title = product.Title;
                productToEdit.Price = product.Price;
                productToEdit.ProfileImageUrl = product.ProfileImageUrl;

                _productRepository.Save();

                return RedirectToAction("Index");
            }
        }


    }
}
