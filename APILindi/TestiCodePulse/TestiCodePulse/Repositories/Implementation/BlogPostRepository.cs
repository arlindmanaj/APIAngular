using Microsoft.EntityFrameworkCore;
using TestiCodePulse.Data;
using TestiCodePulse.Models.Domain;
using TestiCodePulse.Repositories.Interface;

namespace TestiCodePulse.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private ApplicationDbContext dbContext;

        public BlogPostRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
           await dbContext.BlogPosts.AddAsync(blogPost);
           await dbContext.SaveChangesAsync();
           return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
           return await dbContext.BlogPosts.ToListAsync();
        }
    }
}
