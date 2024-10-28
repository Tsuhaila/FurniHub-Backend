using AutoMapper;
using FurniHub.Models.UserModels;
using FurniHub.Models.UserModels.DTOs;
using FurniHub.Models.Categories;
using FurniHub.Models.Categories.DTOs;
using FurniHub.Models.ProductModels;
using FurniHub.Models.ProductModels.DTOs;

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
            
            
        }
    }
}
