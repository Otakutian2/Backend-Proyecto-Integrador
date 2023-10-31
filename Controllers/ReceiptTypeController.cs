using Mapster;
using Microsoft.AspNetCore.Mvc;
using proyecto_backend.Interfaces;
using proyecto_backend.Models;
using proyecto_backend.Schemas;

namespace proyecto_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceiptTypeController : Controller
    {
        private readonly IReceiptType _receiptTypeService;

        public ReceiptTypeController(IReceiptType receiptTypeService)
        {
            _receiptTypeService = receiptTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReceiptTypeGet>>> GetReceiptType()
        {
            List<ReceiptTypeGet> listReceiptType = (await _receiptTypeService.GetAll()).Adapt<List<ReceiptTypeGet>>();

            return Ok(listReceiptType);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReceiptTypeGet>> GetReceiptType(int id)
        {
            ReceiptType receiptType = await _receiptTypeService.GetById(id);

            if (receiptType == null)
            {
                return NotFound("Tipo de Comprobante no encontrado");
            }

            ReceiptTypeGet receiptTypeGet = receiptType.Adapt<ReceiptTypeGet>();

            return Ok(receiptTypeGet);
        }

        [HttpPost]
        public async Task<ActionResult<ReceiptTypeGet>> CreateReceiptType([FromBody] ReceiptTypePrincipal receiptType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ReceiptType newReceiptType = receiptType.Adapt<ReceiptType>();

            await _receiptTypeService.CreateReceiptType(newReceiptType);

            var receiptTypeGet = (await _receiptTypeService.GetById(newReceiptType.Id)).Adapt<ReceiptTypeGet>();

            return CreatedAtAction(nameof(GetReceiptType), new { id = receiptTypeGet.Id }, receiptTypeGet);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ReceiptTypeGet>> UpdateReceiptType(int id, [FromBody] ReceiptTypePrincipal receiptUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ReceiptType receiptType = await _receiptTypeService.GetById(id);

            if (receiptType == null)
            {
                return NotFound("Tipo de Comprobante no encontrado");
            }

            receiptType.Name = receiptUpdate.Name;

            await _receiptTypeService.UpdateReceiptType(receiptType);

            var receiptTypeGet = (await _receiptTypeService.GetById(id)).Adapt<ReceiptTypeGet>();

            return Ok(receiptTypeGet);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReceiptType(int id)
        {
            var receiptType = await _receiptTypeService.GetById(id);

            if (receiptType == null)
            {
                return NotFound("Tipo de Comprobante no encontrado");
            }
            await _receiptTypeService.DeleteReceiptType(receiptType);

            return NoContent();
        }
    }
}
