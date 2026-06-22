using InvoiceApp.Application.DTOs.Clients;
using InvoiceApp.Application.DTOs.Common;
using InvoiceApp.Application.Interfaces;
using InvoiceApp.Domain.Entities;
using InvoiceApp.Domain.Interfaces;

namespace InvoiceApp.Application.Services;

public class ClientService : IClientService
{
    private readonly IUnitOfWork _uow;

    public ClientService(IUnitOfWork uow) => _uow = uow;

    public async Task<PagedResult<ClientResponse>> GetClients(int page, int pageSize, string? search = null)
    {
        var (items, total) = await _uow.Clients.GetPagedAsync(page, pageSize, search);
        return new PagedResult<ClientResponse>
        {
            Items      = items.Select(ToResponse),
            TotalCount = total,
            Page       = page,
            PageSize   = pageSize
        };
    }

    public async Task<ClientResponse> GetClientById(int id)
    {
        var client = await _uow.Clients.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Cliente {id} no encontrado.");
        return ToResponse(client);
    }

    public async Task<ClientResponse> CreateClient(CreateClientRequest request)
    {
        if (await _uow.Clients.DocumentExistsAsync(request.DocumentNumber))
            throw new InvalidOperationException($"Ya existe un cliente con el documento '{request.DocumentNumber}'.");

        var client = new Client
        {
            Name           = request.Name.Trim(),
            DocumentType   = request.DocumentType,
            DocumentNumber = request.DocumentNumber.Trim(),
            Email          = request.Email.Trim(),
            Phone          = request.Phone.Trim(),
            Address        = request.Address.Trim(),
            CreatedAt      = DateTime.UtcNow
        };

        await _uow.Clients.AddAsync(client);
        await _uow.SaveChangesAsync();
        return ToResponse(client);
    }

    public async Task<ClientResponse> UpdateClient(int id, UpdateClientRequest request)
    {
        var client = await _uow.Clients.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Cliente {id} no encontrado.");

        if (await _uow.Clients.DocumentExistsAsync(request.DocumentNumber, excludeId: id))
            throw new InvalidOperationException($"Ya existe otro cliente con el documento '{request.DocumentNumber}'.");

        client.Name           = request.Name.Trim();
        client.DocumentType   = request.DocumentType;
        client.DocumentNumber = request.DocumentNumber.Trim();
        client.Email          = request.Email.Trim();
        client.Phone          = request.Phone.Trim();
        client.Address        = request.Address.Trim();

        _uow.Clients.Update(client);
        await _uow.SaveChangesAsync();
        return ToResponse(client);
    }

    public async Task DeleteClient(int id)
    {
        var client = await _uow.Clients.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Cliente {id} no encontrado.");

        client.IsActive = false;
        _uow.Clients.Update(client);
        await _uow.SaveChangesAsync();
    }

    private static ClientResponse ToResponse(Client c) => new()
    {
        Id             = c.Id,
        Name           = c.Name,
        DocumentType   = c.DocumentType,
        DocumentNumber = c.DocumentNumber,
        Email          = c.Email,
        Phone          = c.Phone,
        Address        = c.Address,
        IsActive       = c.IsActive,
        CreatedAt      = c.CreatedAt
    };
}
