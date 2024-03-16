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
    public class NewsManager : INewsManager
    {
        private readonly INewsRepository _newsRepository;
        private readonly IMapper _mapper;

        public NewsManager(INewsRepository newsRepository, IMapper mapper)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
        }

        public void CreateNews(NewsDTO news)
        {
            news.NewsStatus = 2;
            _newsRepository.CreateNews(_mapper.Map<NewsDTO, News>(news));
            _newsRepository.Save();
        }

        public async Task<bool> UpdateNews(NewsDTO newsDTO)
        {
            News? news = await _newsRepository.GetNews(newsDTO.NewsId.Value);
            if (news == null) return false;
            news = _mapper.Map<NewsDTO, News>(newsDTO);

            _newsRepository.UpdateNews(news);
            _newsRepository.Save();
            return true;
        }

        public async Task<bool> DeleteNews(int id)
        {
            News? news = await _newsRepository.GetNews(id);
            if (news == null) return false;

            news.NewsStatus = 0;
            _newsRepository.UpdateNews(news);
            _newsRepository.Save();
            return true;
        }

        public async Task<bool> Accept(int id)
        {
            News? news = await _newsRepository.GetNews(id);
            if (news == null) return false;

            news.NewsStatus = 1;
            _newsRepository.UpdateNews(news);
            _newsRepository.Save();
            return true;
        }

        public async Task<NewsDTO?> GetNews(int id)
        {
            News? news = await _newsRepository.GetNews(id);
            return news is null ? null : _mapper.Map<News, NewsDTO>(news);
        }

        public async Task<List<NewsDTO>> GetNews()
        {
            List<NewsDTO> newsDTO = [];
            List<News> news = await _newsRepository.GetNews();
            foreach(News newsEntity in news)
            {
                newsDTO.Add(_mapper.Map<News, NewsDTO>(newsEntity));
            }
            return newsDTO;
        }

        public async Task<List<NewsDTO>> SearchNews(string searchString)
        {
            List<NewsDTO> newsDTO = [];
            List<News> news = await _newsRepository.SearchNews(searchString);
            foreach (News newsEntity in news)
            {
                newsDTO.Add(_mapper.Map<News, NewsDTO>(newsEntity));
            }
            return newsDTO;
        }

        public async Task<List<NewsDTO>> GetNewsLastest(int count)
        {
            List<NewsDTO> newsDTO = [];
            List<News> news = await _newsRepository.GetNews();
            news = news.OrderByDescending(x => x.CreateDate).Take(count).ToList();
            newsDTO.AddRange(news.Select(_mapper.Map<News, NewsDTO>));
            return newsDTO;
        }
    }
}
