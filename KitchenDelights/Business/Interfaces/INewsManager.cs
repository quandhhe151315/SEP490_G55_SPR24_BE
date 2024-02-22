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

        Task<NewsDTO?> GetNews(int id);
    }
}
