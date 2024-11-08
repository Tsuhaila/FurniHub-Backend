using AutoMapper;
using FurniHub.Models.UserModels;
using FurniHub.Models.UserModels.DTOs;
using FurniHub.Models.Categories;
using FurniHub.Models.Categories.DTOs;
using FurniHub.Models.ProductModels;
using FurniHub.Models.ProductModels.DTOs;
using FurniHub.Models.OrderModels;
using FurniHub.Models.OrderModels.DTOs;
using FurniHub.Models.WishlistModels;
using FurniHub.Models.WishlistModels.DTOs;

namespace FurniHub.Mapping
{
    public class AppMapper:Profile
    {
        public AppMapper()
        {
            CreateMap<User,UserRegisterDTO>().ReverseMap();
            CreateMap<User,OutPutUser>().ReverseMap();
            CreateMap<Category,CategoryRequestDTO>().ReverseMap();
            CreateMap<Category, CategoryResponseDTO>().ReverseMap();
            CreateMap<Product,ProductResponseDTO>().ReverseMap();
            CreateMap<Product,ProductRequestDTO>().ReverseMap();
            CreateMap<Order,OrderResponseDTO>().ReverseMap();
            CreateMap<Wishlist,WishlistResponseDTO>().ReverseMap();
            CreateMap<Wishlist, WishlistRequestDTO>().ReverseMap();
          


        }
    }
}
