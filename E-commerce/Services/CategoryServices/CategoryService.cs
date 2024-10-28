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
        public async Task<bool> CreateCategory(CategoryRequestDTO categoryDTO)
        {
            try
            {
                var newCategory = _mapper.Map<Category>(categoryDTO);
                await _context.Categories.AddAsync(newCategory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

           
            
        }
        public async Task<bool> UpdateCategory(int id,CategoryRequestDTO categoryDTO)
        {
            var product=_context.Categories.FirstOrDefault(c => c.Id == id);
            if (product != null)
            {
                product.Name = categoryDTO.Name;
            }
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<List<CategoryResponseDTO>> GetAllCategories()
        {
            var categories=await _context.Categories.ToListAsync();
            var mappedCategories=_mapper.Map<List<CategoryResponseDTO>>(categories);
            return mappedCategories;
        }
        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category != null)
            {
               _context.Categories.Remove(category);
            }
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
