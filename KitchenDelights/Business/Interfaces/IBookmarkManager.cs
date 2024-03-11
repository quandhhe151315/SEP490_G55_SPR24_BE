using Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IBookmarkManager
    {
        Task<BookmarkDTO?> GetBookmarkOfUser(int id);

        Task<bool> AddRecipeToBookmark(int userId, int recipeId);

        Task<bool> RemoveRecipeFromBookmark(int userId, int recipeId);
    }
}
