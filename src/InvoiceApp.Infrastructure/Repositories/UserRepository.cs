using InvoiceApp.Domain.Entities;
using InvoiceApp.Domain.Interfaces;
using InvoiceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _ctx;

    public UserRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<(IEnumerable<User> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
    {
        var query = _ctx.Users
                        .Where(u => u.IsActive)
                        .OrderBy(u => u.Name)
                        .AsNoTracking();

        var total = await query.CountAsync();
        var items = await query
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

        return (items, total);
    }

    public Task<User?> GetByIdAsync(int id) =>
        _ctx.Users.FirstOrDefaultAsync(u => u.Id == id);

    public Task<User?> GetByUsernameAsync(string username) =>
        _ctx.Users.FirstOrDefaultAsync(u => u.Username == username);

    public Task<bool> UsernameExistsAsync(string username) =>
        _ctx.Users.AnyAsync(u => u.Username == username);

    public async Task AddAsync(User user) => await _ctx.Users.AddAsync(user);

    public void Update(User user) => _ctx.Users.Update(user);
}
