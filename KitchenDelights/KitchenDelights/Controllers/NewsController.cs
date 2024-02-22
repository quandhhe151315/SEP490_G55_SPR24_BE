using Business.DTO;
using Business.Interfaces;
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
        public async Task<IActionResult> Get(int id)
        {
            NewsDTO? news = await _newsManager.GetNews(id);
            return news == null ? NotFound("News doesn't exist!") : Ok(news);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewsDTO news)
        {
            news.CreateDate = DateTime.Now;
            news.NewsStatus = false;
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
            if (!isUpdated) return StatusCode(StatusCodes.Status500InternalServerError, "Update failed!");
            return Ok();
        }

    }
}
