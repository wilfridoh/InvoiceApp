using InvoiceApp.Application.DTOs.Invoices;
using InvoiceApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InvoicePaymentsController : ControllerBase
{
    private readonly IInvoicePaymentService _paymentService;

    public InvoicePaymentsController(IInvoicePaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet("by-invoice/{invoiceId:int}")]
    public async Task<IActionResult> GetByInvoiceId(int invoiceId)
    {
        var result = await _paymentService.GetByInvoiceId(invoiceId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMany([FromBody] ReplaceInvoicePaymentsRequest request)
    {
        var result = await _paymentService.ReplaceByInvoiceId(request.InvoiceId, request.Payments);
        return Ok(result);
    }

    [HttpPut("by-invoice/{invoiceId:int}")]
    public async Task<IActionResult> ReplaceByInvoiceId(
        int invoiceId,
        [FromBody] List<CreateInvoicePaymentRequest> payments)
    {
        var result = await _paymentService.ReplaceByInvoiceId(invoiceId, payments);
        return Ok(result);
    }

    public class ReplaceInvoicePaymentsRequest
    {
        public int InvoiceId { get; set; }
        public List<CreateInvoicePaymentRequest> Payments { get; set; } = [];
    }
}
