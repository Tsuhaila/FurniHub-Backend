using FurniHub.Models.ProductModels;
using FurniHub.Models.UserModels;

namespace FurniHub.Models.WishlistModels
{
    public class Wishlist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public virtual User? Users { get; set; }
        public virtual Product Products { get; set; }
    }
}
