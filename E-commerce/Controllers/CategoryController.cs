using FurniHub.Models.Categories.DTOs;
using FurniHub.Services.CategoryServices;
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
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories=await _categoryService.GetAllCategories();
            return Ok(categories);

        }
        [HttpPost]
        public async Task<IActionResult>CreateCategory(CategoryRequestDTO categoryDTO)
        {
           await _categoryService.CreateCategory(categoryDTO);
            return Ok("successfully created");
        }
        [HttpPut]
        public async Task<IActionResult>UpdateCategory(int id,CategoryRequestDTO categoryDTO)
        {
            await _categoryService.UpdateCategory(id,categoryDTO);
            return Ok("successfully updated");
        }
        [HttpDelete]
        public async Task<IActionResult>DeleteCategory(int id)
        {
            await _categoryService.DeleteCategory(id);
            return Ok("successfully deleted");
        }
    }
}
