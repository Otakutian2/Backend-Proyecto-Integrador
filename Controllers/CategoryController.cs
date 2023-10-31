using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proyecto_backend.Interfaces;
using proyecto_backend.Models;
using proyecto_backend.Schemas;

namespace proyecto_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory _categoryService;
        private readonly IDish _dishService;

        public CategoryController(ICategory categoryService, IDish dishService)
        {
            _categoryService = categoryService;
            _dishService = dishService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryGet>>> GetCategory()
        {
            return Ok((await _categoryService.GetAll()).Adapt<List<CategoryGet>>());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryGet>> GetCategory(string id)
        {
            var category = await _categoryService.GetById(id);

            if (category == null)
            {
                return NotFound("Categoría de Plato no encontrada");
            }

            var categoryGet = category.Adapt<CategoryGet>();

            return Ok(categoryGet);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<CategoryGet>> UpdateCategory(string id, [FromBody] CategoryPrincipal category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updateCategory = await _categoryService.GetById(id);

            if (updateCategory == null)
            {
                return NotFound("Categoría de Plato no encontrada");
            }

            var isNotNameUnique = !await _categoryService.IsNameUnique(category.Name, updateCategory.Id);

            if (isNotNameUnique)
            {
                return Conflict("El nombre de categoría ya está en uso");
            }

            updateCategory.Name = category.Name;
            await _categoryService.UpdateCategory(updateCategory);

            var getCategory = (await _categoryService.GetById(id)).Adapt<CategoryGet>();

            return Ok(getCategory);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<CategoryGet>> CreateCategory([FromBody] CategoryPrincipal category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isNotNameUnique = !await _categoryService.IsNameUnique(category.Name);

            if (isNotNameUnique)
            {
                return Conflict("El nombre de categoría ya está en uso");
            }

            var newCategory = category.Adapt<Category>();

            await _categoryService.CreateCategory(newCategory);

            var getCategory = (await _categoryService.GetById(newCategory.Id)).Adapt<CategoryGet>();

            return CreatedAtAction(nameof(GetCategory), new { id = getCategory.Id }, getCategory);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> DeleteCategory(string id)
        {
            var category = await _categoryService.GetById(id);

            if (category == null)
            {
                return NotFound("Categoría de Plato no encontrada");
            }

            await _categoryService.DeteleCategory(category);

            return NoContent();
        }

        [HttpGet("{id}/number-dish")]
        public async Task<ActionResult<int>> GetNumberDishInCategory(string id)
        {
            var categoryCount = await _categoryService.Count(c => c.Id == id);

            if (categoryCount == 0)
            {
                return NotFound("Categoría de Plato no encontrada");
            }

            var count = await _dishService.Count(d => d.CategoryId == id);

            return Ok(count);
        }
    }
}
