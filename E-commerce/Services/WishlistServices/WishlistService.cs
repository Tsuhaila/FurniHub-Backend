using AutoMapper;
using FurniHub.Models.WishlistModels;
using FurniHub.Models.WishlistModels.DTOs;
using FurniHub.Services.JwtServices;
using Microsoft.EntityFrameworkCore;

namespace FurniHub.Services.WishlistServices
{
    public class WishlistService:IWishlistService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        public WishlistService(ApplicationDbContext context,IMapper mapper ,IJwtService jwtService)
        {
            _context = context;
            _mapper = mapper;
            _jwtService = jwtService;
            
        }
        public async Task<string>AddOrRemoveWishlist(string token,int productId)
        {
            try
            {
                var userId=_jwtService.GetUserIdFromToken(token);
                if (userId == 0)
                {
                    throw new Exception("user is not valid");
                }
                var isExist = await _context.Wishlist.Include(w => w.Products)
               .FirstOrDefaultAsync(p => p.UserId == userId && p.ProductId == productId);
                if (isExist == null)
                {
                    WishlistRequestDTO requestDTO = new WishlistRequestDTO
                    {
                        ProductId = productId,
                        UserId = userId,
                    };
                    var newWishlist = _mapper.Map<Wishlist>(requestDTO);
                    await _context.AddAsync(newWishlist);
                    await _context.SaveChangesAsync();
                    return "item added to wishlist";

                }
                _context.Wishlist.Remove(isExist);
                await _context.SaveChangesAsync();
                return "item removed from wishlist";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }           
        }
        public async Task<List<WishlistResponseDTO>>GetWishlist(string token)
        {
            try
            {
                var userId = _jwtService.GetUserIdFromToken(token);
                if (userId == 0)
                {
                    throw new Exception("user is not valid");
                }
                var items = await _context.Wishlist.Include(w => w.Products)
                .ThenInclude(p => p.Category).Where(w => w.UserId == userId).ToListAsync();
                if (items != null)
                {
                    var Wishlist = items.Select(i => new WishlistResponseDTO
                    {
                        Id = i.Id,
                        ProductName = i.Products.Name,
                        CategoryName = i.Products.Category.Name,
                        Price = i.Products.Price,
                        ProductImage = i.Products.Image,
                        ProductDescription = i.Products.Description,
                    }).ToList();
                    return Wishlist;
                }
                else
                {
                    return new List<WishlistResponseDTO>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
