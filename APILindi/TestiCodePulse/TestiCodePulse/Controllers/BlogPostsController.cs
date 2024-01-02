using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestiCodePulse.Models.Domain;
using TestiCodePulse.Models.DTO;
using TestiCodePulse.Repositories.Implementation;
using TestiCodePulse.Repositories.Interface;

namespace TestiCodePulse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ICategoryRepository categoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.categoryRepository = categoryRepository;
        }
        //POST : {apibaseurl}/api/blogposts
        [HttpPost]

        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto request)
        {
            // dto to domain
            var blogpost = new BlogPost
            {
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Content = request.Content,
                Author = request.Author,
                PublishedDate = request.PublishedDate,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };

            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await categoryRepository.GetById(categoryGuid);
                if (existingCategory is not null)
                {
                    blogpost.Categories.Add(existingCategory);
                }
            }



            blogpost = await blogPostRepository.CreateAsync(blogpost);
            // Convert domain to dto

            var response = new BlogPostDto
            {
                Title = blogpost.Title,
                ShortDescription = blogpost.ShortDescription,
                Content = blogpost.Content,
                Author = blogpost.Author,
                PublishedDate = blogpost.PublishedDate,
                FeaturedImageUrl = blogpost.FeaturedImageUrl,
                IsVisible = blogpost.IsVisible,
                UrlHandle = blogpost.UrlHandle,
                Categories = blogpost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,

                }).ToList()

            };
            return Ok(response);
        }

        // GET : {apibaseurl}/api/blogposts
        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var blogposts = await blogPostRepository.GetAllAsync();
            //Converti domain n Dto

            var response = new List<BlogPostDto>();
            foreach (var blogpost in blogposts)
            {
                response.Add(new BlogPostDto
                {
                    Title = blogpost.Title,
                    ShortDescription = blogpost.ShortDescription,
                    Content = blogpost.Content,
                    Author = blogpost.Author,
                    PublishedDate = blogpost.PublishedDate,
                    Id = blogpost.Id,
                    IsVisible = blogpost.IsVisible,
                    UrlHandle = blogpost.UrlHandle,
                    FeaturedImageUrl = blogpost.FeaturedImageUrl,
                    Categories = blogpost.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle,

                    }).ToList()
                });
            }

            return Ok(response);
        }

        // GET (1) : {apibaseurl}/api/blogposts/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
            //Get the blog post from repo
            var blogPost = await blogPostRepository.GetByIdAsync(id);
            if (blogPost == null)
            {
                return NotFound();

            }
            //Convert domain to dto
            var response = new BlogPostDto
            {
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                Author = blogPost.Author,
                PublishedDate = blogPost.PublishedDate,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,

                }).ToList()
            };
            return Ok(response);

        }

        //PUT : {apibaseurl}/api/blogposts/{id}
        [HttpPut]
        [Route("{id:Guid}")]

        public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id, UpdateBlogPostRequestDto request)
        {
            // Convert Dto To domain
            var blogPost = new BlogPost
            {
                Id = id,
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Content = request.Content,
                Author = request.Author,
                PublishedDate = request.PublishedDate,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };

            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await categoryRepository.GetById(categoryGuid);
                if (existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            // Repo to UpdateBlogPostDomain
            var updatedBlogPost = await blogPostRepository.UpdateAsync(blogPost);
            if (updatedBlogPost == null)
            {
                return NotFound();
            }
            var response = new BlogPostDto
            {
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                Author = blogPost.Author,
                PublishedDate = blogPost.PublishedDate,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,

                }).ToList()

            };
            return Ok(response);
        }
        //Delete : {apibaseurl}/api/blogposts/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {
            var deletedBlogPost = await blogPostRepository.DeleteAsync(id);
            if (deletedBlogPost == null)
            {
                return NotFound();
            }
            //Convert Domain Model to DTO 

            var response = new BlogPostDto
            {
                Id = deletedBlogPost.Id,
                Title = deletedBlogPost.Title,
                ShortDescription = deletedBlogPost.ShortDescription,
                Content = deletedBlogPost.Content,
                Author = deletedBlogPost.Author,
                PublishedDate = deletedBlogPost.PublishedDate,
                FeaturedImageUrl = deletedBlogPost.FeaturedImageUrl,
                IsVisible = deletedBlogPost.IsVisible,
                UrlHandle = deletedBlogPost.UrlHandle
            };

            return Ok(response);
        }
    }
}
