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
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethod _paymentMethodService;
        private readonly IReceipt _receiptService;

        public PaymentMethodController(IPaymentMethod paymentMethodService, IReceipt receiptService)
        {
            _paymentMethodService = paymentMethodService;
            _receiptService = receiptService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentMethodGet>>> GetPaymentMethod()
        {
            return Ok((await _paymentMethodService.GetAll()).Adapt<List<PaymentMethodGet>>());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentMethodGet>> GetPaymentMethod(int id)
        {
            var paymentMethod = await _paymentMethodService.GetById(id);

            if (paymentMethod == null)
            {
                return NotFound("Método de Pago no encontrado");
            }

            var paymentMethodGet = paymentMethod.Adapt<PaymentMethodGet>();

            return Ok(paymentMethodGet);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<PaymentMethodGet>> CreatePaymentMethod([FromBody] PaymentMethodPrincipal paymentMethod)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var IsNotPaymentMethodUnique = !await _paymentMethodService.IsPaymentMethodUnique(paymentMethod.Name);

            if (IsNotPaymentMethodUnique)
            {
                return Conflict("El nombre de método de pago ya está en uso");
            }

            var newPaymentMethod = paymentMethod.Adapt<PaymentMethod>();

            await _paymentMethodService.CreatePaymentMethod(newPaymentMethod);

            var getPaymentMethod = (await _paymentMethodService.GetById(newPaymentMethod.Id)).Adapt<PaymentMethodGet>();

            return CreatedAtAction(nameof(GetPaymentMethod), new { id = getPaymentMethod.Id }, getPaymentMethod);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<PaymentMethodGet>> UpdatePaymentMethod(int id, [FromBody] PaymentMethodPrincipal paymentMethodUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paymentMethod = await _paymentMethodService.GetById(id);

            if (paymentMethod == null)
            {
                return NotFound("Método de Pago no encontrado");
            }

            var IsNotPaymentMethodUnique = !await _paymentMethodService.IsPaymentMethodUnique(paymentMethodUpdate.Name, paymentMethod.Id);

            if (IsNotPaymentMethodUnique)
            {
                return Conflict("El nombre de método de pago ya está en uso");
            }

            paymentMethod.Name = paymentMethodUpdate.Name;

            await _paymentMethodService.UpdatePaymentMethod(paymentMethod);

            var getPaymentMethod = (await _paymentMethodService.GetById(id)).Adapt<PaymentMethodGet>();

            return Ok(getPaymentMethod);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> DeletePaymentMethod(int id)
        {
            var paymentMethod = await _paymentMethodService.GetById(id);

            if (paymentMethod == null)
            {
                return NotFound("Método de Pago no encontrado");
            }

            await _paymentMethodService.DeletePaymentMethod(paymentMethod);

            return NoContent();
        }

        [HttpGet("{id}/number-receipts-details")]
        public async Task<ActionResult<int>> GetNumberReceiptDetailsInPaymentMethod(int id)
        {
            var paymentMethodCount = await _paymentMethodService.Count(pm => pm.Id == id);

            if (paymentMethodCount == 0)
            {
                return NotFound("Método de Pago no encontrado");
            }

            var count = await _receiptService.ReceiptDetailsCount(rd => rd.PaymentMethodId == id);

            return Ok(count);
        }
    }
}
