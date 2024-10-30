using FurniHub.Models.CartModels;
using FurniHub.Models.CartModels.DTOs;
using FurniHub.Models.ProductModels;
using FurniHub.Services.JwtServices;
using Microsoft.EntityFrameworkCore;
using System;

namespace FurniHub.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly IJwtService _jwtService;
        private readonly ApplicationDbContext _context;
        private readonly string _HostUrl;
        private readonly IConfiguration _configuration;
        public CartService(IJwtService jwtService, ApplicationDbContext context, IConfiguration configuration)
        {
            _jwtService = jwtService;
            _context = context;
            _configuration = configuration;
            _HostUrl = _configuration["HostUrl:Url"];


        }
        public async Task<List<CartResponseDTO>> GetCartItems(string token)
        {
            try
            {
                int userId = _jwtService.GetUserIdFromToken(token);
                if (userId == 0)
                {
                    throw new Exception("User with id doesn't exist !");
                }
                var user = await _context.Cart
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(p => p.UserId == userId);
                if (user == null)
                {
                    return [];
                }
                if (user != null)
                {
                    var cartItems = user.CartItems.Select(ci => new CartResponseDTO
                    {
                        Id = ci.Id,
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
                throw new Exception("something went wrong",ex);
            }
        }
        public async Task<string> AddToCart(string token, int productId)
        {
            try
            {
                int userId = _jwtService.GetUserIdFromToken(token);
                if (userId == 0) throw new Exception("user not valid");

                var user = await _context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                    .FirstOrDefaultAsync(u => u.Id == userId);

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
                CartItem?existingCartProduct=user.Cart.CartItems.FirstOrDefault(ci=>ci.ProductId== productId);
                if (existingCartProduct != null)
                {
                    existingCartProduct.Quantity++;
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
                return "item added to cart";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "something went wrong";
            }
        }

        public async Task<string> RemoveFromCart(string token,int productId)
        {
            try
            {
                var userId = _jwtService.GetUserIdFromToken(token);
                if (userId == 0)
                {
                    throw new Exception("user id is not valid !!");
                }
                var user = await _context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                    .FirstOrDefaultAsync(u => u.Id == userId);
                var product = await _context.Products.FirstOrDefaultAsync(p=>p.Id == productId);

                if(user!=null && product != null)
                {
                    var item = user.Cart.CartItems.FirstOrDefault(ci =>ci.ProductId == productId);
                    if (item != null)
                    {
                        _context.CartItems.Remove(item);
                        await _context.SaveChangesAsync();
                        return "item removed from cart";
                    }
                   
                }
                return "something went wrong";
            }
            catch(Exception ex)
            {               
                throw new Exception(ex.Message);
            }
        }
        public async Task<string> IncreaseQuantity(string token,int productId)
        {
            try
            {
                var userId = _jwtService.GetUserIdFromToken(token);
                if (userId == 0)
                {
                    throw new Exception("A user with the current token is not found !");
                }
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
                        item.Quantity++;
                        await _context.SaveChangesAsync();
                        return "item Quantity is incremented";
                    }
                    
                }
                return "something went wrong";

            }catch(Exception ex)
            {                
                throw new Exception(ex.Message);
            }        
        }

        public async Task<string>DecreaseQuantity(string token,int productId)
        {
            try
            {
                var userId = _jwtService.GetUserIdFromToken(token);
                if (userId == 0)
                {
                    throw new Exception("A user with the current token is not found !");
                }
                var user = await _context.Users.Include(u => u.Cart).ThenInclude(c => c.CartItems).FirstOrDefaultAsync(u => u.Id == userId);
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
                if (user != null && product != null)
                {
                    var item = user.Cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                    if (item != null)
                    {
                       item.Quantity= item.Quantity >= 1 ? item.Quantity - 1 : item.Quantity;
                        if (item.Quantity == 0)
                        {
                            _context.CartItems.Remove(item);
                            
                            
                        }
                        await _context.SaveChangesAsync();
                        return "quantity of item is decreased";
                    }
                }
                return "something went wrong";
                    

            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }   
    }
}
        
                

            
            





                    



        
    

