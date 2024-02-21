using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly string _folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/");
        
        [HttpPost]
        public async Task<IActionResult> Image(IFormFile image, string type)
        {
            if (!IsImage(image))
            {
                return BadRequest("Please upload an image!");
            }

            ImageTypes _type;

            if(!Enum.TryParse(type.ToLower(), out _type))
            {
                return BadRequest("Wrong image type!");
            }

            string subFolder = Path.Combine(_folderPath, _type.ToString());

            if (!Directory.Exists(subFolder))
            {
                Directory.CreateDirectory(subFolder);
            }

            string filename = image.FileName;

            string uniqueName = WebEncoders.Base64UrlEncode(Guid.NewGuid().ToByteArray()) + filename;

            var imagePath = Path.Combine(subFolder, uniqueName);

            await image.CopyToAsync(new FileStream(imagePath, FileMode.Create));

            return Ok($"{Request.Scheme}://{Request.Host.Value}/images/{_type}/{uniqueName}");
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

        public enum ImageTypes
        {
            avatar,
            recipe,
            news,
            blog,
            advertisement
        }
    }
}
