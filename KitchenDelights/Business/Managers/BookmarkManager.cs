using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers
{
    public class BookmarkManager : IBookmarkManager
    {
        private readonly IBookmarkRepository _bookmarkRepository;
        private readonly IMapper _mapper;

        public BookmarkManager(IBookmarkRepository bookmarkRepository, IMapper mapper)
        {
            _bookmarkRepository = bookmarkRepository;
            _mapper = mapper;
        }

        public async Task AddRecipeToBookmark(int userId, int recipeId)
        {
            _bookmarkRepository.AddRecipeToBookmark(userId, recipeId);
            _bookmarkRepository.Save();
        }

        public async Task<BookmarkDTO?> GetBookmarkOfUser(int id)
        {
            User? user = await _bookmarkRepository.GetBookmarkOfUser(id);
            return user == null ? null : _mapper.Map<User, BookmarkDTO>(user);
        }
    }
}
