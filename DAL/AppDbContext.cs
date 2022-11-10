using FlowerSite.Models;
using Microsoft.EntityFrameworkCore;

namespace Flower_site.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderImg> SliderImgs { get; set; }
        public DbSet<Bio> Bios { get; set; }
    }
}
