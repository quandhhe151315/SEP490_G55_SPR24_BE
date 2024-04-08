using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using KitchenDelights.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NewsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly INewsManager _newsManager;

        public NewsController(IConfiguration configuration, INewsManager newsManager)
        {
            _configuration = configuration;
            _newsManager = newsManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? id)
        {
            if (id == null)
            {
                List<NewsDTO> newsDTO = await _newsManager.GetNews();
                if(newsDTO.Count == 0) return NotFound("There's no news here!");
                return Ok(newsDTO);
            }
            if (id != null && id < 0) return BadRequest("Invalid Id");
            NewsDTO? news = await _newsManager.GetNews(id.Value);
            return news == null ? NotFound("News doesn't exist!") : Ok(news);
        }

        [HttpGet]
        public async Task<IActionResult> UserGet()
        {
            List<NewsDTO> newsDTO = await _newsManager.GetNews();
            newsDTO = newsDTO.Where(x => x.NewsStatus == 1)
                             .ToList();
            return Ok(newsDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string? search)
        {
            List<NewsDTO> newsDTOs;
            if(search.IsNullOrEmpty())
            {
                newsDTOs = await _newsManager.GetNews();
            } else
            {
                newsDTOs = await _newsManager.SearchNews(StringHelper.Process(search));
            }
            return Ok(newsDTOs);
        }

        [HttpGet]
        public async Task<IActionResult> Lastest(int count)
        {
            if(count < 0) return BadRequest("Invalid count value!");
            return Ok(await _newsManager.GetNewsLastest(count));
        }

        [Authorize(Roles = "Administrator,Writer")]
        [HttpPost]
        public async Task<IActionResult> Create(NewsDTO news)
        {
            if(news.NewsTitle.IsNullOrEmpty()) return BadRequest("News title should not be empty!");
            if(news.FeaturedImage.IsNullOrEmpty()) return BadRequest("News image should not be empty!");
            news.CreateDate = DateTime.Now;
            news.NewsStatus = 2;
            try
            {
                _newsManager.CreateNews(news);
                return Ok();
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "Administrator,Writer")]
        [HttpPut]
        public async Task<IActionResult> Update(NewsDTO news)
        {
            if(news.NewsId != null && news.NewsId.Value < 0) return BadRequest("Invalid news Id");
            NewsDTO? newsDTO = await _newsManager.GetNews(news.NewsId.Value);
            if (newsDTO == null) return NotFound("News doesn't exist!");
            if(news.NewsTitle.IsNullOrEmpty()) return BadRequest("News title should not be empty!");
            if(news.FeaturedImage.IsNullOrEmpty()) return BadRequest("News image should not be empty!");
            bool isUpdated = await _newsManager.UpdateNews(news);
            return !isUpdated ? StatusCode(500, "Update failed!") : Ok();
        }

        [Authorize(Roles = "Administrator,Moderator,Writer")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if(id < 0) return BadRequest("Invalid Id");
            NewsDTO? newsDTO = await _newsManager.GetNews(id);
            if (newsDTO == null) return NotFound("News doesn't exist!");

            bool isDeleted = await _newsManager.DeleteNews(id);
            return !isDeleted ? StatusCode(500, "Delete failed!") : Ok();
        }

        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPatch]
        public async Task<IActionResult> Accept(int id)
        {
            if(id < 0) return BadRequest("Invalid Id");
            bool isAccepted = await _newsManager.Accept(id);
            return isAccepted ? Ok() : StatusCode(500, "Approve News failed!");
        }
    }
}
