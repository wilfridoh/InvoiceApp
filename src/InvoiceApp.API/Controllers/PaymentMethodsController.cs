using InvoiceApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentMethodsController : ControllerBase
{
    private readonly IPaymentMethodService _paymentMethodService;

    public PaymentMethodsController(IPaymentMethodService paymentMethodService)
    {
        _paymentMethodService = paymentMethodService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllActive()
    {
        var result = await _paymentMethodService.GetAllActive();
        return Ok(result);
    }
}
