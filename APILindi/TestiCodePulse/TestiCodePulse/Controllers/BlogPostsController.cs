using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestiCodePulse.Models.Domain;
using TestiCodePulse.Models.DTO;
using TestiCodePulse.Repositories.Interface;

namespace TestiCodePulse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
        }
        //POST : {apibaseurl}/api/blogposts
        [HttpPost]
        
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto request)
        {
            // dto to domain
            var blogpost = new BlogPost {
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Content = request.Content,
                Author = request.Author,
                PublishedDate = request.PublishedDate,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                UrlHandle = request.UrlHandle,
            };
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
                UrlHandle = blogpost.UrlHandle
                
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
                });
            }

           return Ok(response);
        }
    }
}
