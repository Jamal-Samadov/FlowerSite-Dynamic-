using Flower_site.DAL;
using FlowerSite.Areas.admin.Models;
using FlowerSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlowerSite.Areas.admin.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public CategoryController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {

            var categories = await _dbContext.Categories.ToListAsync();
            return View(categories);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id is null || id == 0) return NotFound();

            var category = await _dbContext.Categories.FindAsync(id);

            if (category == null) return NotFound();
            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateModel category)
        {
            if (!ModelState.IsValid) return View();

            var existName = await _dbContext.Categories.AnyAsync(x => x.Name.ToLower().Equals(category.Name.ToLower()));

            if (existName)
            {
                ModelState.AddModelError("name", "Eyni ad daxil edilə bilməz");
                return View();
            }

            var categoryEntity = new Category
            {
                Name = category.Name,
                Description = category.Description,
                Row = category.Row,

            };

            await _dbContext.Categories.AddAsync(categoryEntity);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id == 0) return NotFound();

            var category = await _dbContext.Categories.FindAsync(id);

            if (category == null) return NotFound();

            return View(new CategoryUpdateModel
            {
                Name=category.Name,
                Description=category.Description,
                Row=category.Row,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, CategoryUpdateModel model)
        {
            if (id is null || id == 0) return NotFound();
            if (!ModelState.IsValid) return View();

            var category = await _dbContext.Categories.FindAsync(id);

            if (category.Id != model.Id) return BadRequest();

            if (category == null) return NotFound();


            var isExistName = await _dbContext.Categories.AnyAsync(x => x.Id == id);
            //if (isExistName)
            //{
            //    ModelState.AddModelError("Name", "Eyni adı 2 dəfə təkrar yaza bilməzsən");
            //    return View(model);
            //}
            category.Name = model.Name;
            category.Description = model.Description;


            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();
            if (!ModelState.IsValid) return NotFound();

            var category = await _dbContext.Categories.FindAsync(id);

            if (category is null) return NotFound();

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
