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

        Task AddRecipeToBookmark(int userId, int recipeId);

        Task RemoveRecipeFromBookmark(int userId, int recipeId);
    }
}
