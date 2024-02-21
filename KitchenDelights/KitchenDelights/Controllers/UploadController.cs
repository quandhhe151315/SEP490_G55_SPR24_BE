using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly string _folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/");
        
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

            string subfolder = Path.Combine(_folderPath, "images", _type.ToString());

            if (!Directory.Exists(subfolder))
            {
                Directory.CreateDirectory(subfolder);
            }

            string filename = image.FileName;

            string uniqueName = WebEncoders.Base64UrlEncode(Guid.NewGuid().ToByteArray()) + filename;

            var imagePath = Path.Combine(subfolder, uniqueName);

            await image.CopyToAsync(new FileStream(imagePath, FileMode.Create));

            return Ok($"{Request.Scheme}://{Request.Host.Value}/images/{_type}/{uniqueName}");
        }

        [HttpPost]
        public async Task<IActionResult> Document(IFormFile document)
        {
            if(!IsDocument(document))
            {
                return BadRequest("Please upload a document!");
            }

            string subfolder = Path.Combine(_folderPath, "documents");

            if (!Directory.Exists(subfolder))
            {
                Directory.CreateDirectory(subfolder);
            }

            string filename = document.FileName;

            string uniqueName = WebEncoders.Base64UrlEncode(Guid.NewGuid().ToByteArray()) + filename;

            var documentPath = Path.Combine(subfolder, uniqueName);

            await document.CopyToAsync(new FileStream(documentPath, FileMode.Create));

            return Ok($"{Request.Scheme}://{Request.Host.Value}/documents/{uniqueName}");
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

        public static bool IsDocument(IFormFile file)
        {
            if (!file.ContentType.Equals("application/msword", StringComparison.CurrentCultureIgnoreCase)
                && !file.ContentType.Equals("application/vnd.openxmlformats-officedocument.wordprocessingml.document", StringComparison.CurrentCultureIgnoreCase)
                && !file.ContentType.Equals("application/vnd.oasis.opendocument.text", StringComparison.CurrentCultureIgnoreCase)
                && !file.ContentType.Equals("application/pdf", StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }

            if (!Path.GetExtension(file.FileName).Equals(".doc", StringComparison.CurrentCultureIgnoreCase)
                && !Path.GetExtension(file.FileName).Equals(".docx", StringComparison.CurrentCultureIgnoreCase)
                && !Path.GetExtension(file.FileName).Equals(".odt", StringComparison.CurrentCultureIgnoreCase)
                && !Path.GetExtension(file.FileName).Equals(".pdf", StringComparison.CurrentCultureIgnoreCase))
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
