using AutoMapper;
using FurniHub.Models.Categories;
using FurniHub.Models.Categories.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FurniHub.Services.CategoryServices
{
    public class CategoryService:ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(ApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            
        }
        public async Task<string> CreateCategory(CategoryRequestDTO categoryDTO)
        {
            try
            {
                var newCategory = _mapper.Map<Category>(categoryDTO);
                await _context.Categories.AddAsync(newCategory);
                await _context.SaveChangesAsync();
                return "category added";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }           
        }
        public async Task<string> UpdateCategory(int id,CategoryRequestDTO categoryDTO)
        {
            try
            {
                var product = _context.Categories.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    product.Name = categoryDTO.Name;
                }
                await _context.SaveChangesAsync();
                return "category updated";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<CategoryResponseDTO>> GetAllCategories()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();
                var mappedCategories = _mapper.Map<List<CategoryResponseDTO>>(categories);
                return mappedCategories;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }          
        }
        public async Task<string> DeleteCategory(int id)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
                if (category != null)
                {
                    _context.Categories.Remove(category);
                }
                await _context.SaveChangesAsync();
                return "category deleted";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }

    }
}
