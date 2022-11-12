using System.ComponentModel.DataAnnotations;

namespace FlowerSite.ViewModels
{
    public class RegisterViewModel
    {
        [Required, StringLength(30)]
        public string Name { get; set; }

        [Required, StringLength(30)]
        public string Surname { get; set; }

        [Required, StringLength(20)]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Required]
        public bool Terms { get; set; }

    }
}
