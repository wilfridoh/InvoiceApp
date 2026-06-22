using InvoiceApp.Application.DTOs.Invoices;
using InvoiceApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InvoiceDetailsController : ControllerBase
{
    private readonly IInvoiceDetailService _detailService;

    public InvoiceDetailsController(IInvoiceDetailService detailService)
    {
        _detailService = detailService;
    }

    [HttpGet("by-invoice/{invoiceId:int}")]
    public async Task<IActionResult> GetByInvoiceId(int invoiceId)
    {
        var result = await _detailService.GetByInvoiceId(invoiceId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMany([FromBody] ReplaceInvoiceDetailsRequest request)
    {
        var result = await _detailService.ReplaceByInvoiceId(request.InvoiceId, request.Details);
        return Ok(result);
    }

    [HttpPut("by-invoice/{invoiceId:int}")]
    public async Task<IActionResult> ReplaceByInvoiceId(
        int invoiceId,
        [FromBody] List<CreateInvoiceDetailRequest> details)
    {
        var result = await _detailService.ReplaceByInvoiceId(invoiceId, details);
        return Ok(result);
    }

    public class ReplaceInvoiceDetailsRequest
    {
        public int InvoiceId { get; set; }
        public List<CreateInvoiceDetailRequest> Details { get; set; } = [];
    }
}
