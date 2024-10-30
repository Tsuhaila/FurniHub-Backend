using FurniHub.Models.ProductModels.DTOs;
using FurniHub.Services.ProductServices;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("All-Products")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProducts();
                return Ok(products);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
           

        }

        [HttpGet("Product-By/{id}")]
        public async Task<IActionResult>GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductById(id);
                return Ok(product);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
           
        }

        [HttpGet("Products-By-CategoryId")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            try
            {
                var products = await _productService.GetProductsByCategory(categoryId);
                return Ok(products);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("Products-By-CategoryName")]
        public async Task<IActionResult>GetProductsByCategoryName(string categoryName)
        {
            try
            {
                var products = await _productService.GetProductsByCategoryName(categoryName);
                return Ok(products);

            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
          
        }

        [Authorize(Roles ="Admin")]
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

        [Authorize(Roles ="Admin")]
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

        [Authorize(Roles ="Admin")]
        [HttpDelete("{id}")]       
        public async Task<IActionResult>DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                return Ok("deleted succefully");

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);    
            }
           
        }
    }
}
