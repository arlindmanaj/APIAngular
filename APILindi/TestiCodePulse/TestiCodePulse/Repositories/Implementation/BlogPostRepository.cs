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

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlogPost =  await dbContext.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
            if (existingBlogPost != null)
            {
                dbContext.BlogPosts.Remove(existingBlogPost);
                await dbContext.SaveChangesAsync();
            }
            return existingBlogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
           return await dbContext.BlogPosts.Include(x=> x.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
         return  await dbContext.BlogPosts.Include(x=> x.Categories).FirstOrDefaultAsync(x=> x.Id == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost = await dbContext.BlogPosts.Include(blogPost => blogPost.Categories).FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlogPost == null)
            {

                return null;

            }
            // Update Bp

                dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);

            // UpdateCategories
            if (existingBlogPost.Categories == null)
            {
                existingBlogPost.Categories = new List<Category>(); // Initialize if null
            }
            else
            {
                existingBlogPost.Categories.Clear(); // Clear existing categories
            }
            existingBlogPost.Categories = blogPost.Categories;

            await dbContext.SaveChangesAsync();
            return existingBlogPost;
        }
    }
}
