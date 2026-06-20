using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Domain.Interfaces;

public interface IUserRepository
{
    Task<(IEnumerable<User> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
    Task<User?>  GetByIdAsync(int id);
    Task<User?>  GetByUsernameAsync(string username);
    Task<bool>   UsernameExistsAsync(string username);
    Task         AddAsync(User user);
    void         Update(User user);
}
