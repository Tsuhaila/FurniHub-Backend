using FurniHub.Models.ApiResponseModel;
using FurniHub.Models.ProductModels.DTOs;
using FurniHub.Models.UserModels;
using FurniHub.Services.ProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FurniHub.Controllers
{
    [Route("api/products")]
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
            try
            {
                var products = await _productService.GetAllProducts();
                return Ok(new APIResponse<IEnumerable<ProductResponseDTO>>(HttpStatusCode.OK, true, "fetch products successfully", products));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<string>(HttpStatusCode.InternalServerError, false, ex.Message, null));
            }


        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductById(id);
                if (product == null)
                {
                    return NotFound(new APIResponse<ProductResponseDTO>(HttpStatusCode.NotFound, false, "product with ID {id} not found.", product));
                }
                return Ok(new APIResponse<ProductResponseDTO>(HttpStatusCode.OK, true, "fetched product with ID {id}", product));


            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<string>(HttpStatusCode.InternalServerError, false, ex.Message, null));
            }

        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            try
            {
                var products = await _productService.GetProductsByCategory(categoryId);
                return Ok(new APIResponse<IEnumerable<ProductResponseDTO>>(HttpStatusCode.OK, true, "fetch products by category id", products));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<string>(HttpStatusCode.InternalServerError, false, ex.Message, null));
            }
        }

        [HttpGet("category-name/{categoryName}")]
        public async Task<IActionResult> GetProductsByCategoryName(string categoryName)
        {
            try
            {
                var products = await _productService.GetProductsByCategoryName(categoryName);
                return Ok(new APIResponse<IEnumerable<ProductResponseDTO>>(HttpStatusCode.OK, true, "fetch products by category", products));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<string>(HttpStatusCode.InternalServerError, false, ex.Message, null));
            }

        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] ProductRequestDTO productDTO, IFormFile image)
        {
            try
            {
                var res= await _productService.CreateProduct(productDTO, image);
                return Ok(new APIResponse<string>(HttpStatusCode.OK, true, "product added",res));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<string>(HttpStatusCode.InternalServerError, false, ex.Message, null));
            }

        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductRequestDTO productDTO, IFormFile image)
        {
            try
            {
                var res=await _productService.UpdateProduct(id, productDTO, image);
   
                return Ok(new APIResponse<string>(HttpStatusCode.OK, true, "products updated", res));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<string>(HttpStatusCode.InternalServerError, false, ex.Message, null));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var res=await _productService.DeleteProduct(id);
                return Ok(new APIResponse<string>(HttpStatusCode.OK, true, "products deleted", res));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<string>(HttpStatusCode.InternalServerError, false, ex.Message, null));
            }

        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts(string search)
        {
            try
            {
                var res = await _productService.SearchProducts(search);
                return Ok(new APIResponse<IEnumerable<ProductResponseDTO>>(HttpStatusCode.OK, true, "fetched searched products", res));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse<string>(HttpStatusCode.InternalServerError, false, ex.Message, null));
            }
        }
        [HttpGet("paginated")]
        public async Task<IActionResult> ProductPagination(int page,int limit)
        {
            try
            {
                var res = await _productService.ProductPagination(page, limit);
                return Ok(new APIResponse<IEnumerable<ProductResponseDTO>>(HttpStatusCode.OK, true, "paginated", res));

            }
            catch(Exception ex)
            {
                return StatusCode(500, new APIResponse<string>(HttpStatusCode.InternalServerError, false, ex.Message, null));
            }
        }
    }
}
