using FlowerSite.Models;
using FlowerSite.Models.IdentityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Flower_site.DAL
{
    public class AppDbContext : IdentityDbContext<User>
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
