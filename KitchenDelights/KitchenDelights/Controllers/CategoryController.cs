using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using KitchenDelights.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICategoryManager categoryManager;

        public CategoryController(IConfiguration configuration, ICategoryManager categoryManagers)
        {
            _configuration = configuration;
            categoryManager = categoryManagers;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDTO category)
        {
            if (category.CategoryType == null)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, "Please enter all require input");
            }
            else
            {
                try
                {
                    categoryManager.CreateCategory(category);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            return Ok(category);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(CategoryDTO category)
        {
            if (category.CategoryType == null)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, "Please enter all require input");
            }
            else
            {
                try
                {
                    if (categoryManager.GetCategoryById(category.CategoryId) == null)
                    {
                        return NotFound("Category not exist");
                    }
                    categoryManager.UpdateCategory(category);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            return Ok("Update sucessfully");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            try
            {
                if (categoryManager.GetCategoryById(categoryId) == null)
                {
                    return NotFound("Category not exist");
                }
                categoryManager.DeleteCategory(categoryId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok("Delete sucessfully");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategoy()
        {
            List<CategoryDTO> categories = categoryManager.GetAllCategories();
            if (categories.Count <= 0)
            {
                return NotFound("There are not exist any category in database");
            }
            return Ok(categories);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            CategoryDTO category = categoryManager.GetCategoryById(categoryId);
            if (category.CategoryId == 0)
            {
                return NotFound("Category not exist");
            }
            return Ok(category);
        }
    }
}
