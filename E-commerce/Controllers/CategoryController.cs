using FurniHub.Models.Categories.DTOs;
using FurniHub.Services.CategoryServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FurniHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            
        }
        [HttpGet("All-Categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategories();
                return Ok(categories);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("Add-Category")]
        public async Task<IActionResult>AddCategory(CategoryRequestDTO categoryDTO)
        {
            try
            {
                var res=await _categoryService.CreateCategory(categoryDTO);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpPut("Update-Category/{id}")]
        public async Task<IActionResult>UpdateCategory(int id,CategoryRequestDTO categoryDTO)
        {
            try
            {
                var res=await _categoryService.UpdateCategory(id, categoryDTO);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpDelete("Delete-Category/{id}")]
        public async Task<IActionResult>DeleteCategory(int id)
        {
            try
            {
                var res=await _categoryService.DeleteCategory(id);
                return Ok(res);

            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
           
        }
    }
}
