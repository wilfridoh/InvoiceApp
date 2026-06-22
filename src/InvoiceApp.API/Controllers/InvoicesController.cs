using InvoiceApp.Application.DTOs.Invoices;
using InvoiceApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoicesController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? invoiceNumber = null,
        [FromQuery] int? clientId = null,
        [FromQuery] int? sellerId = null,
        [FromQuery] DateTime? dateFrom = null,
        [FromQuery] DateTime? dateTo = null,
        [FromQuery] string? status = null)
    {
        var filters = new InvoiceListFilters
        {
            Page = page,
            PageSize = pageSize,
            InvoiceNumber = invoiceNumber,
            ClientId = clientId,
            SellerId = sellerId,
            DateFrom = dateFrom,
            DateTo = dateTo,
            Status = status
        };

        var result = await _invoiceService.GetInvoices(filters);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var invoice = await _invoiceService.GetInvoiceById(id);
        return Ok(invoice);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateInvoiceRequest request)
    {
        var created = await _invoiceService.CreateInvoice(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPatch("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        await _invoiceService.Cancel(id);
        return NoContent();
    }
}
