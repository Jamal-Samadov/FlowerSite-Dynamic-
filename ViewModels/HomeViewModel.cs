using FlowerSite.Models;

namespace FlowerSite.ViewModels
{
    public class HomeViewModel
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<SliderImg> SliderImgs { get; set; } = new List<SliderImg>();
        public List<Slider>? Slider { get; set; } 
    }
}
  