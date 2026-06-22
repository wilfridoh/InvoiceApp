using InvoiceApp.Application.DTOs.Clients;
using InvoiceApp.Application.DTOs.Common;

namespace InvoiceApp.Application.Interfaces;

public interface IClientService
{
    Task<PagedResult<ClientResponse>> GetClients(int page, int pageSize, string? search = null);
    Task<ClientResponse> GetClientById(int id);
    Task<ClientResponse> CreateClient(CreateClientRequest request);
    Task<ClientResponse> UpdateClient(int id, UpdateClientRequest request);
    Task DeleteClient(int id);
}
