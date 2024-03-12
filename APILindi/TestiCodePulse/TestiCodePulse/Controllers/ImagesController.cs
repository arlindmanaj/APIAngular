using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using TestiCodePulse.Models.Domain;
using TestiCodePulse.Models.DTO;
using TestiCodePulse.Repositories.Interface;

namespace TestiCodePulse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        //GET: {apibaseurl}/api/images/
        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            //call image repo to get all images
            var images = await imageRepository.GetAll();
            //Convert domain to dto
            var response = new List<BlogImageDto>();
            foreach(var image in images)
            {
                response.Add(new BlogImageDto
                {
                    Id = image.Id,
                    Title = image.Title,
                    DateCreated = image.DateCreated,
                    FileExtension = image.FileExtension,
                    FileName = image.FileName,
                    Url = image.Url


                });

            }

            return Ok(response);

        }

        // POST: {apibaseurl}/api/images\
        [HttpPost]

        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string title)
        {
            ValidateFileUpload(file);
            if(ModelState.IsValid)
            {
                // vazhdo me file upload
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    DateCreated = DateTime.Now,

                };
                blogImage = await imageRepository.Upload(file, blogImage);
                // Convert domain to dto 

                var response = new BlogImageDto
                {
                    Id = blogImage.Id,
                    Title = blogImage.Title,
                    DateCreated = blogImage.DateCreated,
                    FileExtension = blogImage.FileExtension,
                    FileName = blogImage.FileName,
                    Url = blogImage.Url

                };
                return Ok(response);
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" , ".PNG"};
            if(!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower())) 
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }
            if(file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File Size cannot be more than 10mb");
            }
        }
        // POST: {apibaseurl}/api/images
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteImage([FromRoute] Guid id)
        {
            var deletedImage = await imageRepository.Delete(id);
            if (deletedImage == null)
            {
                return NotFound();
            }
            var response = new BlogImageDto
            {
                Id = deletedImage.Id,
                
            };

            return Ok(response);
        }
    }
}
