using Business.DTO;
using Business.Interfaces;
using Business.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAdvertisementManager _advertisementManager;

        public AdvertisementController(IConfiguration configuration, IAdvertisementManager advertisementManager)
        {
            _configuration = configuration;
            _advertisementManager = advertisementManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAdvertismentById(int? id)
        {
            if (id == null)
            {
                List<AdvertisementDTO> advertisementDTOs = await _advertisementManager.GetAdvertisements();
                if (advertisementDTOs.Count == 0) return NotFound("There are not have any advertisement in database");
                return Ok(advertisementDTOs);
            }
            AdvertisementDTO? advertisement = await _advertisementManager.GetAdvertisementById(id.Value);
            return advertisement == null ? NotFound("Advertisement doesn't exist!") : Ok(advertisement);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdvertisementDTO advertisementDTO)
        {
            advertisementDTO.AdvertisementStatus = 0;
            try
            {
                _advertisementManager.CreateAdvertisement(advertisementDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(AdvertisementDTO advertisementDTO)
        {
            AdvertisementDTO? AdvertisementDTO = await _advertisementManager.GetAdvertisementById(advertisementDTO.AdvertisementId.Value);
            if (AdvertisementDTO == null) return NotFound("Advertisement doesn't exist!");

            bool isUpdated = await _advertisementManager.UpdateAdvertisement(advertisementDTO);
            return !isUpdated ? StatusCode(StatusCodes.Status500InternalServerError, "Update failed!") : Ok("Update sucess!");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            AdvertisementDTO? AdvertisementDTO = await _advertisementManager.GetAdvertisementById(id);
            if (AdvertisementDTO == null) return NotFound("Advertisement doesn't exist!");

            bool isDeleted = await _advertisementManager.DeleteAdvertisement(id);
            return !isDeleted ? StatusCode(StatusCodes.Status500InternalServerError, "Delete failed!") : Ok("Delete sucess!");
        }
    }
}
