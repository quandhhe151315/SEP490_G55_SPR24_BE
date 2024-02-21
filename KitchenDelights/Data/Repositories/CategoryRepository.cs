using Data.Entity;
using Data.Interfaces;
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

        public List<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();
            try
            {
                categories = _context.Categories.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return categories;
        }

        public Category GetCategoryById(int categoryId)
        {
            Category category = new Category();
            try
            {
                category = _context.Categories.FirstOrDefault(category => category.CategoryId == categoryId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return category;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
