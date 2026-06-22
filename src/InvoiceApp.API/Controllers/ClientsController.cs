using InvoiceApp.Application.DTOs.Clients;
using InvoiceApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    /// <summary>Lista paginada de clientes. Admite búsqueda por nombre, documento o email.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var result = await _clientService.GetClients(page, pageSize, search);
        return Ok(result);
    }

    /// <summary>Obtiene un cliente por ID.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var client = await _clientService.GetClientById(id);
        return Ok(client);
    }

    /// <summary>Crea un nuevo cliente.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClientRequest request)
    {
        var created = await _clientService.CreateClient(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Actualiza un cliente existente.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateClientRequest request)
    {
        var updated = await _clientService.UpdateClient(id, request);
        return Ok(updated);
    }

    /// <summary>Elimina (soft delete) un cliente.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _clientService.DeleteClient(id);
        return NoContent();
    }
}
