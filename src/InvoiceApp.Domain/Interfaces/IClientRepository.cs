using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Domain.Interfaces;

public interface IClientRepository
{
    Task<(IEnumerable<Client> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search = null);
    Task<Client?>  GetByIdAsync(int id);
    Task<bool>     DocumentExistsAsync(string documentNumber, int? excludeId = null);
    Task           AddAsync(Client client);
    void           Update(Client client);
}
