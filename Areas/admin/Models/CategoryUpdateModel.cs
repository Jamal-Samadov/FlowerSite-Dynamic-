using System.ComponentModel.DataAnnotations;

namespace FlowerSite.Areas.admin.Models
{
    public class CategoryUpdateModel
    {
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string? Name { get; set; }

        [Required, MaxLength(150)]
        public string? Description { get; set; }
        public int? Row { get; set; }
    }
}
