using Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ICategoryManager
    {
        Task<bool> CreateCategory(CategoryDTO categoryDTO);
        Task<bool> UpdateCategory(CategoryDTO categoryDTO);
        Task<bool> DeleteCategory(int categoryId);
        Task<List<CategoryDTO>> GetAllCategories();
        Task<CategoryDTO?> GetCategoryById(int categoryId);
        Task<List<CategoryDTO>> GetCategoryByParentId(int? parentId);
    }
}
