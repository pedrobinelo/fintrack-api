using FinTrackAPI.DTOs;
using FinTrackAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinTrackAPI.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet] // GET: api/categories
        public async Task<IActionResult> GetCategories()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out var userId))
                return Unauthorized(new { message = "Token inválido ou userId ausente." });

            var categories = await _categoryService.GetCategoriesByUserIdAsync(userId);
            return Ok(categories);
        }

        [HttpPost] // POST: api/categories
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO dto)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
                return Unauthorized(new { message = "Token inválido ou userId ausente." });

            var newCategory = await _categoryService.CreateCategoryAsync(dto, userId);
            return StatusCode(201, newCategory);
        }
    }
}
