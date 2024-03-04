using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IBookmarkRepository
    {
        Task<User?> GetBookmarkOfUser(int id);

        void AddRecipeToBookmark(int userId, int recipeId);

        void RemoveRecipeFromBookmark(int userId, int recipeId);

        void Save();
    }
}
