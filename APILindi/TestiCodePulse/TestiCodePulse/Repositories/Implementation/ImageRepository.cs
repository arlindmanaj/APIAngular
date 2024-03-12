using Microsoft.EntityFrameworkCore;
using TestiCodePulse.Data;
using TestiCodePulse.Models.Domain;
using TestiCodePulse.Repositories.Interface;

namespace TestiCodePulse.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext dbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<BlogImage> Delete(Guid id)
        {
            var existingImage = await dbContext.BlogImages.FirstOrDefaultAsync(x => x.Id == id);

            if(existingImage  == null)
            {
                return null;
            }
            dbContext.BlogImages.Remove(existingImage);
            await dbContext.SaveChangesAsync();
            return existingImage;
        }

        public async Task<IEnumerable<BlogImage>> GetAll()
        {
            return await dbContext.BlogImages.ToListAsync();
        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            // UPload image to api/images (folderi)
            var localPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");

            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);
            // Update database
            //https://codepulse.com/images/somefilename.jpg
            var httprequest = httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httprequest.Scheme}://{httprequest.Host}{httprequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";

            blogImage.Url = urlPath;
            await dbContext.BlogImages.AddAsync(blogImage);
            await dbContext.SaveChangesAsync();
            return blogImage;
        }
    }
}
