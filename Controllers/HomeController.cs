using Flower_site.DAL;
using FlowerSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace FlowerSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var sliderImgs = _dbContext.SliderImgs.ToList();
            var slider = _dbContext.Sliders.ToList();
            var category = _dbContext.Categories.ToList();
            var product = _dbContext.Products.ToList();

            var homeViewModel = new HomeViewModel
            {
                Slider = slider,
                SliderImgs = sliderImgs,
                Categories = category,
                Products = product,
            };

           
            return View(homeViewModel);
        }

        public IActionResult Search(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return NoContent();
            }
            var products = _dbContext.Products.Where(x => x.Name.ToLower().Contains(searchText.ToLower()))?.ToList();
            return PartialView("_SearchedProductPartial",products);
        }

        public async Task<IActionResult> Basket()
        {

            var basketJson = Request.Cookies["basket"];

            if (basketJson == null) return BadRequest();

            var basketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketJson);

            foreach (var basketViewModel in basketViewModels)
            {
                var product = await _dbContext.Products.Include(x => x.Category).SingleOrDefaultAsync(x => x.Id == basketViewModel.Id);

                if (product != null)
                {
                    basketViewModel.Price = product.Price;
                    basketViewModel.Category = product.Category;
                }

            }

            return Json(basketViewModels);
        }

        public async Task<IActionResult> AddToBasket(int id)
        {
            var product = await _dbContext.Products.Include(x => x.Category).SingleOrDefaultAsync(x => x.Id == id);

            if (product == null)
                return NotFound();

            var basketJson = Request.Cookies["basket"];

            List<BasketViewModel> existBasketViewModels = null;

            if (basketJson != null)
                existBasketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketJson);

            if (existBasketViewModels != null)
            {
                var existBasketViewModel = existBasketViewModels.Where(x => x.Id == product.Id).SingleOrDefault();

                if (existBasketViewModel != null) existBasketViewModel.Count++;
                else
                {
                    existBasketViewModels.Add(new BasketViewModel
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        ImageUrl = product.ImageUrl,
                        Category = product.Category,
                        Count = 1
                    });
                }
            }
            else
            {
                existBasketViewModels = new List<BasketViewModel>
                {
                    new BasketViewModel
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        ImageUrl = product.ImageUrl,
                        Category = product.Category,
                        Count = 1
                    }
                };
            }

            var basketViewModelJson = JsonConvert.SerializeObject(existBasketViewModels, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            Response.Cookies.Append("basket", basketViewModelJson);

            return RedirectToAction(nameof(Index));
        }


    }
}
