using AutoMapper;
using FurniHub.Models.WishlistModels;
using FurniHub.Models.WishlistModels.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FurniHub.Services.WishlistServices
{
    public class WishlistService:IWishlistService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly string _hostUrl;
        public WishlistService(ApplicationDbContext context,IMapper mapper,IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _hostUrl = configuration["HostUrl:Url"];
        }
        public async Task<bool>ToggleWishlistItem(int userId,int productId)
        {
            try
            {
                var existingItem = await _context.Wishlist
                    .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);
                if (existingItem == null)
                {
                   
                    var newWishlistItem = _mapper.Map<Wishlist>(new WishlistRequestDTO
                    {
                        ProductId = productId,
                        UserId = userId,
                    });

                    await _context.Wishlist.AddAsync(newWishlistItem);
                    await _context.SaveChangesAsync();
                    return true;

                }
                _context.Wishlist.Remove(existingItem);
                await _context.SaveChangesAsync();
                return false;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }           
        }
        public async Task<List<WishlistResponseDTO>>GetWishlist(int userId)
        {
            try
            {
                var items = await _context.Wishlist
                    .Include(w => w.Products)
                    .ThenInclude(p => p.Category)
                    .Where(w => w.UserId == userId)
                    .ToListAsync();
                
                    return items.Select(i => new WishlistResponseDTO
                    {
                        Id = i.Id,
                        ProductName = i.Products.Name,
                        CategoryName = i.Products.Category.Name,
                        Price = i.Products.Price,
                        OfferPrice=i.Products.OfferPrice,
                        ProductImage =_hostUrl+ i.Products.Image,
                        ProductDescription = i.Products.Description,
                        ProductId = i.Products.Id.ToString()
                    }).ToList();             
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
