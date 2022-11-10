using Flower_site.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlowerSite.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _dbContext;
        private int _productCount;

        public ProductController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _productCount = _dbContext.Products.Count();
        }
        public IActionResult Index()
        {
            var products = _dbContext.Products.Include(x=>x.Category).Take(4).ToList();
            return View(products);
        }

        public IActionResult GetAll(int? id)
        {
            if (id is null)
            {
                return View();
            }
            ViewBag.productCount = _productCount;
            var products = _dbContext.Products.Include(x => x.Category).Take(4).ToList();
            return PartialView("_ProductPartial", products);
        }


        public IActionResult Details(int? id)
        {
            var product = _dbContext.Products.SingleOrDefault(x => x.Id == id);
            return View(product);
        }

        public IActionResult Partial(int skip)
        {
            if (skip >= _productCount)
            {
                return BadRequest();
            }
            var products = _dbContext.Products.Include(x => x.Category).Take(4).ToList();

            return PartialView("_ProductPartial",products);
        }
    }
}
