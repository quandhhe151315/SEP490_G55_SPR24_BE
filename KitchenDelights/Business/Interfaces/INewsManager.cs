using Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface INewsManager
    {
        void CreateNews(NewsDTO news);

        Task<bool> UpdateNews(NewsDTO news);

        Task<bool> DeleteNews(int id);

        Task<bool> Accept(int id);

        Task<NewsDTO?> GetNews(int id);

        Task<List<NewsDTO>> GetNews();

        Task<List<NewsDTO>> SearchNews(string searchString);
    }
}
