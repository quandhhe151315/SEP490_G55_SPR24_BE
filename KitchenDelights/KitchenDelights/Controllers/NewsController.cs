using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using Microsoft.AspNetCore.Mvc;

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
            if(id == null)
            {
                List<NewsDTO> newsDTO = await _newsManager.GetNews();
                if(newsDTO.Count == 0) return NotFound("There's no news here!");
                return Ok(newsDTO);
            }
            NewsDTO? news = await _newsManager.GetNews(id.Value);
            return news == null ? NotFound("News doesn't exist!") : Ok(news);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewsDTO news)
        {
            news.CreateDate = DateTime.Now;
            news.NewsStatus = 2;
            try
            {
                _newsManager.CreateNews(news);
                return Ok();
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(NewsDTO news)
        {
            NewsDTO? newsDTO = await _newsManager.GetNews(news.NewsId.Value);
            if (newsDTO == null) return NotFound("News doesn't exist!");

            bool isUpdated = await _newsManager.UpdateNews(news);
            return !isUpdated ? StatusCode(StatusCodes.Status500InternalServerError, "Update failed!") : Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            NewsDTO? newsDTO = await _newsManager.GetNews(id);
            if (newsDTO == null) return NotFound("News doesn't exist!");

            bool isDeleted = await _newsManager.DeleteNews(id);
            return !isDeleted ? StatusCode(StatusCodes.Status500InternalServerError, "Delete failed!") : Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> Accept(int id)
        {
            bool isAccepted = await _newsManager.Accept(id);
            return isAccepted ? Ok() : StatusCode(500, "Approve News failed!");
        }
    }
}
