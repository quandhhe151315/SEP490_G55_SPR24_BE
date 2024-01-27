using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly string _folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/");
        
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (!IsImage(file))
            {
                return BadRequest("Please upload an image!");
            }

            string filename = file.FileName;

            string uniqueName = WebEncoders.Base64UrlEncode(Guid.NewGuid().ToByteArray()) + filename;

            var imagePath = Path.Combine(_folderPath, uniqueName);

            await file.CopyToAsync(new FileStream(imagePath, FileMode.Create));

            return Ok($"{Request.Scheme}://{Request.Host.Value}/images/{uniqueName}");
        }

        public static bool IsImage(IFormFile file)
        {
            if (!file.ContentType.Equals("image/jpg", StringComparison.CurrentCultureIgnoreCase)
                && !file.ContentType.Equals("image/jpeg", StringComparison.CurrentCultureIgnoreCase)
                && !file.ContentType.Equals("image/webp", StringComparison.CurrentCultureIgnoreCase)
                && !file.ContentType.Equals("image/png", StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }

            if (!Path.GetExtension(file.FileName).Equals(".jpg", StringComparison.CurrentCultureIgnoreCase)
                && !Path.GetExtension(file.FileName).Equals(".png", StringComparison.CurrentCultureIgnoreCase)
                && !Path.GetExtension(file.FileName).Equals(".webp", StringComparison.CurrentCultureIgnoreCase)
                && !Path.GetExtension(file.FileName).Equals(".jpeg", StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}
