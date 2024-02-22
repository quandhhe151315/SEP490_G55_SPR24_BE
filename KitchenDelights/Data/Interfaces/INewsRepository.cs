using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface INewsRepository
    {
        void CreateNews(News news);

        void UpdateNews(News news);

        void DeleteNews(News news);

        Task<News?> GetNews(int id);

        void Save();
    }
}
