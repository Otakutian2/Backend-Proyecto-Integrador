using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project_backend.Interfaces;
using project_backend.Models;
using project_backend.Schemas;

namespace project_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EstablishmentController : ControllerBase
    {
        private readonly IEstablishment _establishmentService;

        public EstablishmentController(IEstablishment establishmentService)
        {
            _establishmentService = establishmentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstablishmentGet>>> GetEstablishment()
        {
            List<EstablishmentGet> listEstablishment = (await _establishmentService.GetAll()).Adapt<List<EstablishmentGet>>();
            return Ok(listEstablishment);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EstablishmentGet>> GetEstablishment(int id)
        {
            var establishment = await _establishmentService.GetById(id);

            if (establishment == null)
            {
                NotFound("Establecimiento no encontrado");
            }

            var establishmentGet = establishment.Adapt<EstablishmentGet>();

            return Ok(establishmentGet);
        }

        [HttpGet("first")]
        public async Task<ActionResult<EstablishmentGet>> GetFirstEstablishment()
        {
            var establishment = (await _establishmentService.GetFirstOrDefault()).Adapt<EstablishmentGet>();
            return Ok(establishment);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<EstablishmentGet>> UpdateEstablishment(int id, [FromBody] EstablishmentPrincipal establishmentUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var establisment = await _establishmentService.GetById(id);

            if (establisment == null)
            {
                return NotFound("Establecimiento no encontrado");
            }

            establisment.Name = establishmentUpdate.Name;
            establisment.Address = establishmentUpdate.Address;
            establisment.Phone = establishmentUpdate.Phone;
            establisment.Ruc = establishmentUpdate.Ruc;

            await _establishmentService.UpdateEstablishment(establisment);

            var getEstablishment = (await _establishmentService.GetById(id)).Adapt<EstablishmentGet>();

            return Ok(getEstablishment);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<EstablishmentGet>> CreateEstablishment([FromBody] EstablishmentPrincipal establishment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newEstablishment = establishment.Adapt<Establishment>();

            await _establishmentService.CreateEstablishment(newEstablishment);

            var getEstablishment = (await _establishmentService.GetById(newEstablishment.Id)).Adapt<EstablishmentGet>();

            return CreatedAtAction(nameof(GetEstablishment), new { id = getEstablishment.Id }, getEstablishment);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteEstablishment(int id)
        {
            var establishment = await _establishmentService.GetById(id);

            if (establishment == null)
            {
                return NotFound("Establecimiento no encontrado");
            }

            await _establishmentService.DeleteEstablishment(establishment);

            return NoContent();
        }
    }
}
