using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project_backend.Dto;
using project_backend.Interfaces;
using project_backend.Models;
using project_backend.Schemas;

namespace project_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DishController : ControllerBase
    {
        private readonly IDish _dishService;
        private readonly ICategory _categoryService;
        private readonly ICommand _commadService;

        public DishController(IDish dishService, ICategory categoryService, ICommand commandService)
        {
            _dishService = dishService;
            _categoryService = categoryService;
            _commadService = commandService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DishGet>>> GetDish()
        {
            return Ok((await _dishService.GetAll()).Adapt<List<DishGet>>());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DishGet>> GetDish(string id)
        {
            var dish = await _dishService.GetById(id);

            if (dish == null)
            {
                return NotFound("Plato no encontrado");
            }

            var dishGet = dish.Adapt<DishGet>();

            return Ok(dishGet);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<DishGet>> UpdateDish(string id, [FromBody] DishCreate dish)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updateDish = await _dishService.GetById(id);

            if (updateDish == null)
            {
                return NotFound("Plato no encontrado");
            }

            var isNotNameUnique = !await _dishService.IsNameUnique(dish.Name, id);

            if (isNotNameUnique)
            {
                return Conflict("El nombre del plato ya está en uso");
            }

            if (updateDish.CategoryId != dish.CategoryId)
            {
                var count = await _categoryService.Count(c => c.Id == dish.CategoryId);

                if (count == 0)
                {
                    return NotFound("Categoría de Plato no encontrada");
                }

                updateDish.CategoryId = dish.CategoryId;
            }

            updateDish.Name = dish.Name;
            updateDish.Price = dish.Price;
            updateDish.Image = dish.Image;
            await _dishService.UpdateDish(updateDish);

            var getDish = (await _dishService.GetById(id)).Adapt<DishGet>();

            return Ok(getDish);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<DishGet>> CreateDish([FromBody] DishCreate dish)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var count = await _categoryService.Count(c => c.Id == dish.CategoryId);

            if (count == 0)
            {
                return NotFound("Categoría de Plato no encontrada");
            }

            var isNotNameUnique = !await _dishService.IsNameUnique(dish.Name);

            if (isNotNameUnique)
            {
                return Conflict("El nombre del plato ya está en uso");
            }

            var newDish = dish.Adapt<Dish>();
            await _dishService.CreateDish(newDish);

            var getDish = (await _dishService.GetById(newDish.Id)).Adapt<DishGet>();

            return CreatedAtAction(nameof(GetDish), new { id = getDish.Id }, getDish);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteDish(string id)
        {
            var dish = await _dishService.GetById(id);

            if (dish == null)
            {
                return NotFound("Plato no encontrado");
            }

            await _dishService.DeteleDish(dish);

            return NoContent();
        }

        [HttpGet("{id}/number-details-commands")]
        public async Task<ActionResult<int>> GetNumberCommandDetailsInDish(string id)
        {
            var dishCount = await _dishService.Count(d => d.Id == id);

            if (dishCount == 0)
            {
                return NotFound("Plato no encontrado");
            }

            var count = await _commadService.CommandDetailsCount(cd => cd.DishId == id);

            return Ok(count);
        }

        [HttpGet("dish-order-statistics")]
        public async Task<ActionResult<IEnumerable<DishOrderStatistics>>> GetDishOrderStatistics()
        {
            return Ok(await _dishService.GetDishOrderStatistics());
        }

        [HttpGet("by-category/{id}")]
        public async Task<ActionResult<IEnumerable<DishGet>>> GetDishByIdCategory(string id)
        {
            return Ok((await _dishService.GetDishByIdCategory(id)).Adapt<List<DishGet>>());
        }

        [HttpGet("verify-name/{name}/{dishId?}")]
        public async Task<ActionResult> VerifyName(string name, string dishId = null)
        {
            var IsNotNameDishUnique = !await _dishService.IsNameUnique(name);

            if (!string.IsNullOrEmpty(dishId))
            {
                IsNotNameDishUnique = !await _dishService.IsNameUnique(name, dishId);
            }

            if (IsNotNameDishUnique)
            {
                return Conflict("El nombre de plato ya está en uso");
            }

            return Ok(new { isFound = IsNotNameDishUnique });
        }
    }
}
