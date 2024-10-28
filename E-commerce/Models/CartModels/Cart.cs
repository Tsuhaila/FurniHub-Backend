using FurniHub.Models.UserModels;
using System.ComponentModel.DataAnnotations;

namespace FurniHub.Models.CartModels
{
    public class Cart
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="user id is required")]
        public int UserId { get; set; }
        public virtual User 
            User { get; set; }
        public virtual List<CartItem> CartItems { get; set; }


    }
}
