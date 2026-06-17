using FinTrackAPI.Data;
using FinTrackAPI.DTOs;
using FinTrackAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FinTrackAPI.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        // Listar apenas as categorias do usuário autenticado
        public async Task<List<Category>> GetCategoriesByUserIdAsync(int userId)
        {
            return await _context.Categories
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        // Criar uma nova categoria para o usuário autenticado
        public async Task<Category> CreateCategoryAsync(CategoryDTO dto, int userId)
        {
            var newCategory = new Category
            {
                Name = dto.Name,
                UserId = userId
            };
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();
            return newCategory;
        }
    }
}
