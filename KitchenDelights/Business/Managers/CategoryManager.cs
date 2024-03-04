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
        ICategoryRepository _categoryRepository;
        private IMapper _mapper;

        public CategoryManager(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public void CreateCategory(CategoryDTO categoryDTO)
        {
            Category category = new Category();
            category.ParentId = categoryDTO.ParentId;
            category.CategoryName = categoryDTO.CategoryName;
            category.CategoryType = categoryDTO.CategoryType;
            _categoryRepository.CreateCategory(category);
            _categoryRepository.Save();
        }

        public void UpdateCategory(CategoryDTO categoryDTO)
        {
            Category category = _categoryRepository.GetCategoryById(categoryDTO.CategoryId);
            category.ParentId = categoryDTO.ParentId;
            category.CategoryName = categoryDTO.CategoryName;
            category.CategoryType = categoryDTO.CategoryType;
            _categoryRepository.UpdateCategory(category);
            _categoryRepository.Save();
        }

        public void DeleteCategory(int categoryId)
        {
            Category category = _categoryRepository.GetCategoryById(categoryId);
            _categoryRepository.DeleteCategory(category);
            _categoryRepository.Save();
        }

        public List<CategoryDTO> GetAllCategories()
        {
            List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();
            List<Category> categories = _categoryRepository.GetAllCategories();
            if (categories.Count > 0)
            {
                foreach (Category category in categories)
                {
                    CategoryDTO categoryDTO = new CategoryDTO();
                    categoryDTO.CategoryId = category.CategoryId;
                    categoryDTO.ParentId = category.ParentId;
                    categoryDTO.CategoryName = category.CategoryName;
                    categoryDTO.CategoryType = category.CategoryType;
                    categoryDTOs.Add(categoryDTO);
                }
            }
            return categoryDTOs;
        }

        public CategoryDTO GetCategoryById(int categoryId)
        {
            CategoryDTO categoryDTO = new CategoryDTO();
            Category category = _categoryRepository.GetCategoryById(categoryId);
            if (category != null)
            {
                categoryDTO.CategoryId = category.CategoryId;
                categoryDTO.ParentId = category.ParentId;
                categoryDTO.CategoryName = category.CategoryName;
                categoryDTO.CategoryType = category.CategoryType;
            }
            return categoryDTO;
        }

        public List<CategoryDTO> GetCategoryByParentId(int? parentId)
        {
            List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();
            List<Category> categories = _categoryRepository.GetCategoryByParentId(parentId);
            if (categories.Count > 0)
            {
                foreach (Category category in categories)
                {
                    CategoryDTO categoryDTO = new CategoryDTO();
                    categoryDTO.CategoryId = category.CategoryId;
                    categoryDTO.ParentId = category.ParentId;
                    categoryDTO.CategoryName = category.CategoryName;
                    categoryDTO.CategoryType = category.CategoryType;
                    categoryDTOs.Add(categoryDTO);
                }
            }
            return categoryDTOs;
        }
    }
}
