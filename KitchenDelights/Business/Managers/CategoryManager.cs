using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using Data.Interfaces;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers
{
    public class CategoryManager : ICategoryManager
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryManager(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<bool> CreateCategory(CategoryDTO categoryDTO)
        {
            try
            {
                Category category = new Category();

                if (categoryDTO.ParentId == 0 || categoryDTO.ParentId == null)
                {
                    category.ParentId = null;
                }
                else
                {
                    category.ParentId = categoryDTO.ParentId;
                }
                category.CategoryName = categoryDTO.CategoryName;
                category.CategoryType = categoryDTO.CategoryType;
                category.CategoryStatus = 1;
                _categoryRepository.CreateCategory(category);
                _categoryRepository.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateCategory(CategoryDTO categoryDTO)
        {
            Category? category = await _categoryRepository.GetCategoryById(categoryDTO.CategoryId.Value);           
            if (category == null) return false;

            if (categoryDTO.ParentId == 0 || categoryDTO.ParentId == null)
            {
                category.ParentId = null;
            }
            else
            {
                category.ParentId = categoryDTO.ParentId;
            }
            category.CategoryName = categoryDTO.CategoryName;
            category.CategoryType = categoryDTO.CategoryType;
            _categoryRepository.UpdateCategory(category);
            _categoryRepository.Save();
            return true;
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
            Category? category = await _categoryRepository.GetCategoryById(categoryId);
            if (category == null) return false;

            _categoryRepository.DeleteCategory(category);
            _categoryRepository.Save();
            return true;
        }

        public async Task<List<CategoryDTO>> GetAllCategories(bool categoryType)
        {
            List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();
            List<Category> categories = await _categoryRepository.GetAllCategories();
            if (categoryType)
            {
                categories = categories.Where(x => x.CategoryType == true).ToList();
            }
            else
            {
                categories = categories.Where(x => x.CategoryType == false).ToList();
            }
            if (categories.Count > 0)
            {
                foreach (Category category in categories)
                {
                    CategoryDTO categoryDTO = new CategoryDTO();
                    categoryDTO.CategoryId = category.CategoryId;
                    categoryDTO.ParentId = category.ParentId;
                    if (categoryDTO.ParentId != null)
                    {
                        categoryDTO.ParentName = category.Parent.CategoryName;
                    }
                    categoryDTO.CategoryName = category.CategoryName;
                    categoryDTO.CategoryType = category.CategoryType;
                    categoryDTOs.Add(categoryDTO);
                }
            }
            return categoryDTOs;
        }

        public async Task<CategoryDTO?> GetCategoryById(int categoryId)
        {
            Category? category = await _categoryRepository.GetCategoryById(categoryId);
            if (category != null)
            {
                CategoryDTO? categoryDTO = new CategoryDTO();
                categoryDTO.CategoryId = category.CategoryId;
                categoryDTO.ParentId = category.ParentId;
                if (categoryDTO.ParentId != null)
                {
                    categoryDTO.ParentName = category.Parent.CategoryName;
                }
                categoryDTO.CategoryName = category.CategoryName;
                categoryDTO.CategoryType = category.CategoryType;
                return categoryDTO;
            }
            else return null;
        }

        public async Task<List<CategoryDTO>> GetCategoryByParentId(int? parentId, bool categoryType)
        {
            List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();
            List<Category> categories = await _categoryRepository.GetCategoryByParentId(parentId);
            if (categoryType)
            {
                categories = categories.Where(x => x.CategoryType == true).ToList();
            }
            else
            {
                categories = categories.Where(x => x.CategoryType == false).ToList();
            }
            if (categories.Count > 0)
            {
                foreach (Category category in categories)
                {
                    CategoryDTO categoryDTO = new CategoryDTO();
                    categoryDTO.CategoryId = category.CategoryId;
                    categoryDTO.ParentId = category.ParentId;
                    if (categoryDTO.ParentId != null)
                    {
                        categoryDTO.ParentName = category.Parent.CategoryName;
                    }
                    categoryDTO.CategoryName = category.CategoryName;
                    categoryDTO.CategoryType = category.CategoryType;
                    categoryDTOs.Add(categoryDTO);
                }
            }
            return categoryDTOs;
        }
    }
}
