using Data.Entity;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly KitchenDelightsContext _context;

        public CategoryRepository(KitchenDelightsContext context)
        {
            _context = context;
        }
        public void CreateCategory(Category category)
        {
            try
            {
                _context.Categories.Add(category);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateCategory(Category category)
        {
            try
            {
                _context.Categories.Update(category);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void DeleteCategory(Category category)
        {
            try
            {
                if (category != null)
                {
                    _context.Categories.Remove(category);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public async Task<List<Category>> GetAllCategories()
        {
            return await _context.Categories.Include(x => x.Parent).ToListAsync();
        }

        public async Task<Category?> GetCategoryById(int categoryId)
        {
            return await _context.Categories.Include(x => x.Parent).FirstOrDefaultAsync(category => category.CategoryId == categoryId);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task<List<Category>> GetCategoryByParentId(int? parentId)
        {
            return await _context.Categories.Include(x => x.Parent).Where(category => category.ParentId == parentId).ToListAsync();
        }
    }
}
