using FurniHub.Models.CartModels;
using FurniHub.Models.CartModels.DTOs;
using FurniHub.Models.ProductModels;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;
using System;

namespace FurniHub.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _HostUrl;
        private readonly IConfiguration _configuration;
        public CartService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _HostUrl = _configuration["HostUrl:Url"];


        }
        public async Task<List<CartResponseDTO>> GetCartItems(int userId)
        {
            try
            {
                var user = await _context.Cart
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(p => p.UserId == userId);
              
                if (user != null)
                {
                    var cartItems = user.CartItems.Select(ci => new CartResponseDTO
                    {
                        Id = ci.ProductId,
                        ProductName = ci.Product.Name,
                        Quantity = ci.Quantity,
                        Price = ci.Product.Price,
                        OfferPrice = ci.Product.OfferPrice,
                        TotalPrice = (ci.Product.Price * ci.Quantity).ToString(),
                        Image = _HostUrl + ci.Product.Image,
                    }).ToList();
                    return cartItems;
                }
                return new List<CartResponseDTO>();


                }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> AddToCart(int userId, int productId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                    .ThenInclude(ci=>ci.Product)
                    .FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    throw new Exception("user not found");
                }

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            
                if (product == null) throw new Exception($"product with id {productId} not found");

                if (product != null && user != null)
                {
                    if (user.Cart == null)
                    {
                        user.Cart = new Cart
                        {
                            UserId = userId,
                            CartItems = new List<CartItem>()
                        };

                        _context.Cart.Add(user.Cart);
                        await _context.SaveChangesAsync();
                    }

                }
                CartItem?existingCartProduct=user?.Cart?.CartItems.FirstOrDefault(ci=>ci.ProductId== productId);
                if (existingCartProduct != null)
                {
                    return false;
                }
                else
                {
                    CartItem cartItem = new CartItem
                    {
                        CartId = user.Cart.Id,
                        ProductId = productId,
                        Quantity = 1
                    };
                   _context.CartItems.Add(cartItem);
                }
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RemoveFromCart(int userId,int productId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                    .ThenInclude(ci=>ci.Product)
                    .FirstOrDefaultAsync(u => u.Id == userId);
                var product = await _context.Products.FirstOrDefaultAsync(p=>p.Id == productId);

                if(user!=null && product != null)
                {
                    var item = user.Cart.CartItems.FirstOrDefault(ci =>ci.ProductId == productId);
                    if (item != null)
                    {
                        _context.CartItems.Remove(item);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                   
                }
                return false;
            }
            catch(Exception ex)
            {               
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> IncreaseQuantity(int userId,int productId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
                if (user != null && product != null)
                {
                    var item = user.Cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                    if (item != null)
                    {
                        if (item.Quantity < product.Quantity) {
                            item.Quantity++;
                        }
                        else
                        {
                            return false;
                        }
                       
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    
                }
                return false;

            }catch(Exception ex)
            {                
                throw new Exception(ex.Message);
            }        
        }

        public async Task<bool>DecreaseQuantity(int userId,int productId)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Cart).ThenInclude(c => c.CartItems).FirstOrDefaultAsync(u => u.Id == userId);
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
                if (user != null && product != null)
                {
                    var item = user.Cart?.CartItems?.FirstOrDefault(ci => ci.ProductId == productId);
                    if (item != null)
                    {
                       if(item.Quantity > 1)
                        {
                            item.Quantity--;
                        }
                        else
                        {
                            return false;
                        }
                       
                        await _context.SaveChangesAsync();
                       
                        return true;
                    }
                }
                return false;
                    

            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }   
    }
}
        
                

            
            





                    



        
    

