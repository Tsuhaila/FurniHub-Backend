using AutoMapper;
using FurniHub.Models.Categories;
using FurniHub.Models.ProductModels;
using FurniHub.Models.ProductModels.DTOs;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace FurniHub.Services.ProductServices
{
    public class ProductService:IProductService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _HostUrl;
        
        public ProductService(IConfiguration configuration, ApplicationDbContext context,IMapper mapper,IWebHostEnvironment webHostEnvironment)
        {

            _configuration = configuration;
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _HostUrl = _configuration["HostUrl:Url"];            
        }
        public async Task<List<ProductResponseDTO>> GetAllProducts()
        {
            try
            {
                var products = await _context.Products.Include(p => p.Category).ToListAsync();
                return products.Select(p => new ProductResponseDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Image = _HostUrl + p.Image,
                    Category = p.Category.Name,
                    Price = p.Price,
                    OfferPrice = p.OfferPrice,
                    Quantity = p.Quantity,
                    CategoryId=p.CategoryId

                }).ToList();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProductResponseDTO> GetProductById(int id)
        {
            try
            {
                var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
                if (product != null)
                {
                    return new ProductResponseDTO
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Image = _HostUrl + product.Image,
                        Category = product.Category.Name,
                        Price = product.Price,
                        OfferPrice = product.OfferPrice,
                        Quantity = product.Quantity,
                        CategoryId=product.CategoryId
                        

                    };
                }
                return new ProductResponseDTO();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }

        public async Task<List<ProductResponseDTO>>GetProductsByCategory(int categoryId)
        {
            try
            {
                var products = await _context.Products
               .Include(p => p.Category)
               .Where(p => p.CategoryId == categoryId)
               .Select(p => new ProductResponseDTO
               {
                   Id = p.Id,
                   Name = p.Name,
                   Description = p.Description,
                   Image = _HostUrl + p.Image,
                   Category = p.Category.Name,
                   Price = p.Price,
                   OfferPrice = p.OfferPrice,
                   Quantity = p.Quantity

               }).ToListAsync();

                if (products != null)
                {
                    return products;
                }
                return [];

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
           
        }
        public async Task<List<ProductResponseDTO>> GetProductsByCategoryName(string categoryName)
        {
            try
            {
                var products = await _context.Products
               .Include(p => p.Category)
               .Where(c => c.Category.Name == categoryName)
               .Select(p => new ProductResponseDTO
               {

                   Id = p.Id,
                   Name = p.Name,
                   Description = p.Description,
                   Image = _HostUrl + p.Image,
                   Category = p.Category.Name,
                   Price = p.Price,
                   OfferPrice = p.OfferPrice,
                   Quantity = p.Quantity

               }).ToListAsync();
                if (products != null)
                {
                    return products;
                }
                return [];

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
       
        }
        
        public async Task<string> CreateProduct(ProductRequestDTO productDTO,IFormFile image)
        {
            try
            {
                string productImage = null;
                if (image != null && image.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Products", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    productImage = "/Images/Products/" + fileName;
                }
                var product = _mapper.Map<Product>(productDTO);
                product.Image = productImage;
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return "product added";
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }
        public async Task<string> UpdateProduct(int id, ProductRequestDTO productDTO, IFormFile image)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(x => x.Id == id);
                if (product != null)
                {
                    product.Name = productDTO.Name;
                    product.Description = productDTO.Description;
                    product.Price = productDTO.Price;
                    product.OfferPrice = productDTO.OfferPrice;
                    product.CategoryId = productDTO.CategoryId;
                    product.Quantity = productDTO.Quantity;
                    
                }
                if (image != null && image.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Products", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    product.Image = "/Images/Products/" + fileName;
                }
                await _context.SaveChangesAsync();
                return "product updated";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
           
        public async Task<string> DeleteProduct(int id)
        {
            try
            {
                var product =await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
                if (product != null)
                {
                    _context.Products.Remove(product);

                }
                await _context.SaveChangesAsync();
                return "product deleted!!";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);                             
            }          
        }
        public async Task<List<ProductResponseDTO>> SearchProducts(string searchTerm)
        {
            try
            {
                var products = await _context.Products.Include(p => p.Category).Where(p => p.Name.ToLower().Contains(searchTerm.ToLower())).ToListAsync();
                if (products != null)
                {
                    return products.Select(p => new ProductResponseDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Category = p.Category.Name,
                        Description = p.Description,
                        Image = _HostUrl + p.Image,
                        Price = p.Price,
                        OfferPrice = p.OfferPrice,
                        Quantity=p.Quantity,

                    }).ToList();

                }
                return new List<ProductResponseDTO>();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<ProductResponseDTO>>ProductPagination(int page,int limit)
        {
            try
            {
                var products = await _context.Products.Include(p => p.Category)
                    .Skip((page - 1) * limit).Take(limit).ToListAsync();
                if (products != null)
                {
                    return products.Select(p => new ProductResponseDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Category = p.Category.Name,
                        Description = p.Description,
                        Image = _HostUrl + p.Image,
                        Price = p.Price,
                        OfferPrice = p.OfferPrice,
                        Quantity= p.Quantity,

                    }).ToList();

                }
                return new List<ProductResponseDTO>();
                    

            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
