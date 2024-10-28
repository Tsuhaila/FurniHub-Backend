using FurniHub.Models.ProductModels.DTOs;
using FurniHub.Services.ProductServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FurniHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
            
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
           var products= await _productService.GetAllProducts();
            return Ok(products);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult>GetProductById(int id)
        {
           var product=await _productService.GetProductById(id);
            return Ok(product);
        }
        [HttpGet("cateory/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
           var products=await _productService.GetProductsByCategory(categoryId);
            return Ok(products);


        }
        [HttpGet("category/name/{categoryName}")]
        public async Task<IActionResult>GetProductsByCategoryName(string categoryName)
        {
           var products=await _productService.GetProductsByCategoryName(categoryName);
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm]ProductRequestDTO productDTO,IFormFile image)
        {
            try
            {
                await _productService.CreateProduct(productDTO, image);
                return Ok("added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
           
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductRequestDTO productDTO, IFormFile image)
        {
            try
            {
                await _productService.UpdateProduct(id, productDTO, image);
                return Ok("updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("{id}")]
        
        public async Task<IActionResult>DeleteProduct(int id)
        {
           await _productService.DeleteProduct(id);
            return Ok("deleted succefully");
        }
    }
}
