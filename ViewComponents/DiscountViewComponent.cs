using Flower_site.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlowerSite.ViewComponents
{
    public class DiscountViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;
        public DiscountViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var discount = await _dbContext.Products.Where(x => x.Discount != null).ToListAsync();
            return View(discount);
        }


        //public async Task<IViewComponentResult> InvokeAsync()
        //{
        //    var discount = await _dbContext.D.ToListAsync();
        //}  
    }
}
