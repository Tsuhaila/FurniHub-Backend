using FurniHub.Models.Categories.DTOs;
using FurniHub.Services.CategoryServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FurniHub.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            
        }
        [HttpGet]
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

        [Authorize(Roles ="admin")]
        [HttpPost]
        public async Task<IActionResult>AddCategory(CategoryRequestDTO categoryDTO)
        {
            try
            {
                var res=await _categoryService.CreateCategory(categoryDTO);
                return CreatedAtAction(nameof(GetAllCategories), res);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles ="admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult>UpdateCategory(int id,CategoryRequestDTO categoryDTO)
        {
            try
            {
                var res=await _categoryService.UpdateCategory(id, categoryDTO);
                if (res == null)
                {
                    return NotFound($"Category with ID {id} not found.");
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles ="admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteCategory(int id)
        {
            try
            {
                var res=await _categoryService.DeleteCategory(id);
                if (res == null)
                {
                    return NotFound($"Category with ID {id} not found.");
                }
                return Ok(res);

            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
           
        }
    }
}
