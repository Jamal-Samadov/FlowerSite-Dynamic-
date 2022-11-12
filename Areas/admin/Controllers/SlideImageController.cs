using Flower_site.DAL;
using FlowerSite.Areas.admin.Data;
using FlowerSite.Areas.admin.Models;
using FlowerSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace FlowerSite.Areas.admin.Controllers
{
    public class SlideImageController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;
        
        public SlideImageController(AppDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var slideImages = await _dbContext.SliderImgs.ToListAsync();

            return View(slideImages);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SlideImageCreateModel model/*, IFormFile file*/)
        {
        //    int width = 1920;
        //    int height = 617;
        //    string path = await model.Image.GenerateFile(Constans.RootPath);
        //    Image image = Image.FromStream(file.OpenReadStream(), true, true);
        //    var newImage = new Bitmap(width, height);
        //    using(var a = Graphics.FromImage(newImage))
        //    {
        //        a.DrawImage(image, 0, 0, width, height);
        //        newImage.Save(path);
        //    }

            if (!ModelState.IsValid) return View();

            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "Zəhmət olmasa düzgün şəkil yükləyin");
                return View();
            }

            if (!model.Image.ImageAllowed(2))
            {
                ModelState.AddModelError("Image", "Şəkilin ölçüsü 1 mbdan çox ola bilməz");
                return View();
            }

            var unicalFileName = await model.Image.GenerateFile(Constans.RootPath);

            await _dbContext.SliderImgs.AddAsync(new SliderImg
            {
                Name = unicalFileName,
            });
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id == 0) return NotFound();
            if (!ModelState.IsValid) return View();

            var slideImages = await _dbContext.SliderImgs.FindAsync(id);


            return View(new SlideImageUpdateModel
            {
                ImageUrl = slideImages.Name
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SlideImageUpdateModel model)
        {
            if (id is null || id == 0) return NotFound();

            var slideImage = await _dbContext.SliderImgs.FindAsync(id);

            if (slideImage.Id != id) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(new SlideImageUpdateModel
                {
                    ImageUrl = slideImage.Name
                });
            }


            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "Zəhmət olmasa düzgün şəkil yükləyin");
                return View(new SlideImageUpdateModel
                {
                    ImageUrl = slideImage.Name
                });
            }

            if (!model.Image.ImageAllowed(2))
            {
                ModelState.AddModelError("Image", "Şəkilin ölçüsü 1 mbdan çox ola bilməz");
                return View(new SlideImageUpdateModel
                {
                    ImageUrl = slideImage.Name
                });
            }

            var path = Path.Combine(Constans.RootPath, "img", slideImage.Name);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            var unicalFileName = await model.Image.GenerateFile(Constans.RootPath);

            slideImage.Name = unicalFileName;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();
            var slideImage = await _dbContext.SliderImgs.FindAsync(id);

            if(slideImage == null)
            {
                ModelState.AddModelError("Image", "Son şəkil silinə bilməz");
                return View();
            }

            if (slideImage == null) return NotFound();
            if (slideImage.Id != id) return BadRequest();

            var path = Path.Combine(Constans.RootPath, "img", slideImage.Name);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
             _dbContext.SliderImgs.Remove(slideImage);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult CreateMulti()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMulti(SlideImageCreateMultiModel model)
        {

            if (!ModelState.IsValid) return View();

            foreach (var image in model.Images)
            {
                if (!image.IsImage())
                {
                    ModelState.AddModelError("Images", "Zəhmət olmasa düzgün şəkil yükləyin");
                    return View();
                }

                if (!image.ImageAllowed(2))
                {
                    ModelState.AddModelError("Images", $"{image.FileName}-Şəkilin ölçüsü 1 mbdan çox ola bilməz");
                    return View();
                }

                var unicalFileName = await image.GenerateFile(Constans.RootPath);

                await _dbContext.SliderImgs.AddAsync(new SliderImg
                {
                    Name = unicalFileName,
                });


               await _dbContext.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }
    }
}
