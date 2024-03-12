using TestiCodePulse.Models.Domain;

namespace TestiCodePulse.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);

        //I ENUMERABLE se osht list nuk e din sa jon.
        Task<IEnumerable<BlogImage>> GetAll();
        Task<BlogImage> Delete(Guid Id);
    }
}
