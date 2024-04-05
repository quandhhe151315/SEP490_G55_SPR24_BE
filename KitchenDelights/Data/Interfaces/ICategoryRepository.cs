using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ICategoryRepository
    {
        void CreateCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
        Task<List<Category>> GetAllCategories();
        Task<Category?> GetCategoryById(int categoryId);
        Task<List<Category>> GetCategoryByParentId(int? parentId);
        Task<List<Category>> GetCategoryByCategoryType(bool categoryType); 
        void Save();
    }
}
