using Flower_site.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlowerSite.ViewComponents
{
    public class BioViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;
        public BioViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var bio = await _dbContext.Bios.ToListAsync();
            return View(bio);
        }
    }
}
