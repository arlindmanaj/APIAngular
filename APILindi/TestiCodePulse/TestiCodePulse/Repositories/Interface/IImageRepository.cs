using TestiCodePulse.Models.Domain;

namespace TestiCodePulse.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);
    }
}
