using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestiCodePulse.Data;
using TestiCodePulse.Models.Domain;
using TestiCodePulse.Models.DTO;
using TestiCodePulse.Repositories.Interface;

namespace TestiCodePulse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    // https://localhost:xxxx/api/categories 
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;


        // me constructor e lidhim db context me controller
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
        {
            // Map DTO to Domain Model
            // Ktu jena tu e bo ni DTO qe me komuniku me Domainin permes APi ku ja jepim si parameter controllerit (action post)
            // DTO's e n DTO pi lidhim Domainin me property t'tij (name url handle), Id nuk duhet se vulnerable information
            var category = new Category
            {
                
                Name = request.Name,
                UrlHandle = request.UrlHandle,
            };

            await categoryRepository.CreateAsync(category);
            
            

            // Response
            // Domain Model to DTO
            // E lidhem Dto props permes ni variable response qe e qesim n return ok per tna kthyer pergjigje nese qato trija shkojn
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);


        }

        // GET : https://localhost:7112/api/Categories -> Pathi per Angular
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
           var categories = await categoryRepository.GetAllAsync();

            // Map Domain model to DTO -> MI mshef
            var response = new List<CategoryDto>();
            foreach (var category in categories) {
            
                response.Add(new CategoryDto 
                {

                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle= category.UrlHandle

                });
            }
            return Ok(response);
        }

        // GET: https://localhost:7112/api/Categories{id} -> Id e blogit
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryId([FromRoute]Guid id)
        {
            var existingCategory =  await categoryRepository.GetById(id);

            if (existingCategory is null)
            {
                return NotFound();
            }
            var response = new CategoryDto 
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                UrlHandle = existingCategory.UrlHandle

            };

            return Ok(existingCategory);
        }

        // PUT :   https://localhost:7112/api/Categories{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> EditCategory([FromRoute] Guid id, UpdateCategoryRequestDto request )
        {
            // Convert DTO to Domain
            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };
           category = await categoryRepository.UpdateAsync(category);
            if(category == null)
            {
                return NotFound();

            }
            //Convert domain model to DTO
            
            var response = new CategoryDto
            { 
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);
        }
    }
}
