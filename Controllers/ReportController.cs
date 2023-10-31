using Microsoft.AspNetCore.Mvc;
using proyecto_backend.Interfaces;
using System.Net.Mime;

namespace proyecto_backend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        private readonly IReport _reportService;
        private readonly IReceipt _receiptService;

        public ReportController(IReport reportService, IReceipt receiptService)
        {
            _reportService = reportService;
            _receiptService = receiptService;
        }

        [HttpGet("receipt/{id}")]
        public async Task<IActionResult> GetReportReceipt(int id)
        {
            var receipt = await _receiptService.GetById(id);

            if (receipt == null)
            {
                return NotFound("Comprobante de pago no encontrado");
            }

            var reportName = $"Comprobante_{receipt.Id}";
            var reportFileBytes = await _reportService.ReportReceipt(receipt);

            return File(reportFileBytes, MediaTypeNames.Application.Pdf, reportName + ".pdf");
        }
    }
}
