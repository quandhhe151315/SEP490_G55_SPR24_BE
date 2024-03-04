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
        void CreateCategory(CategoryDTO categoryDTO);
        void UpdateCategory(CategoryDTO categoryDTO);
        void DeleteCategory(int categoryId);
        List<CategoryDTO> GetAllCategories();
        CategoryDTO GetCategoryById(int categoryId);
        List<CategoryDTO> GetCategoryByParentId(int? parentId);
    }
}
