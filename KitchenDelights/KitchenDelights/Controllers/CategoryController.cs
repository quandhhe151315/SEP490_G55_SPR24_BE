using Business.DTO;
using Business.Interfaces;
using Business.Managers;
using Data.Entity;
using KitchenDelights.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICategoryManager _categoryManager;

        public CategoryController(IConfiguration configuration, ICategoryManager categoryManagers)
        {
            _configuration = configuration;
            _categoryManager = categoryManagers;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategoy()
        {
            List<CategoryDTO> categories = [];
            try
            {
                categories = await _categoryManager.GetAllCategories();
                if (categories.Count <= 0)
                {
                    return NotFound("There are not exist any category in database");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok(categories);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            try
            {
                if (categoryId != 0)
                {
                    CategoryDTO? category = await _categoryManager.GetCategoryById(categoryId);
                    if (category == null)
                    {
                        return NotFound("Category not exist");
                    }
                    return Ok(category);
                }
                else
                {
                    List<CategoryDTO> categories = await _categoryManager.GetAllCategories();
                    if (categories.Count <= 0)
                    {
                        return NotFound("There are not exist any category in database");
                    }
                    return Ok(categories);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryByParentId(int? parentId)
        {
            List<CategoryDTO> categoryDTOs;
            try
            {
                if (parentId != null)
                {
                    categoryDTOs = await _categoryManager.GetCategoryByParentId(parentId);
                    if (categoryDTOs.Count <= 0)
                    {
                        return NotFound("Category with parentId = " + parentId + " not exist");
                    }
                    return Ok(categoryDTOs);
                }
                else
                {
                    categoryDTOs = await _categoryManager.GetCategoryByParentId(parentId);
                    return Ok(categoryDTOs);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> CreateCategory(CategoryDTO category)
        {

            try
            {
                await _categoryManager.CreateCategory(category);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok("Create successful");
        }

        [HttpPut]
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> UpdateCategory(CategoryDTO category)
        {
            CategoryDTO? categoryDTO = await _categoryManager.GetCategoryById(category.CategoryId.Value);
            if (categoryDTO == null) return NotFound("Category not exist");

            bool isUpdated = await _categoryManager.UpdateCategory(category);
            return !isUpdated ? StatusCode(StatusCodes.Status500InternalServerError, "Update failed!") : Ok("Update sucessful");
        }

        [HttpDelete]
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            CategoryDTO? category = await _categoryManager.GetCategoryById(categoryId);
            if (category == null) return NotFound("Category not exist");

            bool isDeleted = await _categoryManager.DeleteCategory(categoryId);
            return !isDeleted ? StatusCode(StatusCodes.Status500InternalServerError, "Delete failed!") : Ok("Delete sucessful");
        }
    }
}
