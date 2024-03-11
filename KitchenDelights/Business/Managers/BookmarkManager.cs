using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers
{
    public class BookmarkManager : IBookmarkManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public BookmarkManager(IUserRepository userRepository,
                                IRecipeRepository recipeRepository,
                                IMapper mapper)
        {
            _userRepository = userRepository;
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddRecipeToBookmark(int userId, int recipeId)
        {
            User? user = await _userRepository.GetUser(userId);
            Recipe? recipe = await _recipeRepository.GetRecipe(recipeId);
            if (user == null || recipe == null)
            {
                return false;
            }
            user.Recipes.Add(recipe);
            _userRepository.UpdateUser(user);
            _userRepository.Save();
            return true;
        }

        public async Task<BookmarkDTO?> GetBookmarkOfUser(int id)
        {
            User? user = await _userRepository.GetBookmarkOfUser(id);
            return user == null ? null : _mapper.Map<User, BookmarkDTO>(user);
        }

        public async Task<bool> RemoveRecipeFromBookmark(int userId, int recipeId)
        {
            User? user = await _userRepository.GetUser(userId);
            Recipe? recipe = await _recipeRepository.GetRecipe(recipeId);
            if (user == null || recipe == null)
            {
                return false;
            }
            user.Recipes.Remove(recipe);
            _userRepository.UpdateUser(user);
            _userRepository.Save();
            return true;
        }
    }
}
