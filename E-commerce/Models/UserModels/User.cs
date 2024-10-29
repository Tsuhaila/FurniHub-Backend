using FurniHub.Models.CartModels;
using FurniHub.Models.OrderModels;
using System.ComponentModel.DataAnnotations;

namespace FurniHub.Models.UserModels
{
    public class User
    {
        public int Id { get; set; }
        [Required (ErrorMessage ="username is required")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "email is required")]
        [EmailAddress(ErrorMessage ="Invalid email address.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "password is required")]
        [MinLength(8,ErrorMessage ="password must be atleast 8 characters long.")]
        public string? Password { get; set; }
        public string? Role { get; set; }
        public virtual Cart? Cart { get; set; }
        public virtual List<Order>? Orders { get; set; }

    }
}
