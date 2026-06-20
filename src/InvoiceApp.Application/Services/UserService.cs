using InvoiceApp.Application.DTOs.Common;
using InvoiceApp.Application.DTOs.Users;
using InvoiceApp.Application.Interfaces;
using InvoiceApp.Domain.Entities;
using InvoiceApp.Domain.Interfaces;

namespace InvoiceApp.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork     _uow;
    private readonly IPasswordHasher _hasher;

    public UserService(IUnitOfWork uow, IPasswordHasher hasher)
    {
        _uow    = uow;
        _hasher = hasher;
    }

    public async Task<PagedResult<UserResponse>> GetUsersAsync(int page = 1, int pageSize = 20)
    {
        var (items, total) = await _uow.Users.GetPagedAsync(page, pageSize);
        return new PagedResult<UserResponse>
        {
            Items      = items.Select(ToResponse),
            TotalCount = total,
            Page       = page,
            PageSize   = pageSize
        };
    }

    public async Task<UserResponse?> GetByIdAsync(int id)
    {
        var user = await _uow.Users.GetByIdAsync(id);
        return user is null ? null : ToResponse(user);
    }

    public async Task<UserResponse> CreateAsync(CreateUserRequest request)
    {
        if (await _uow.Users.UsernameExistsAsync(request.Username))
            throw new InvalidOperationException($"El username '{request.Username}' ya está en uso.");

        var user = new User
        {
            Username     = request.Username,
            PasswordHash = _hasher.Hash(request.Password),
            Name         = request.Name,
            Email        = request.Email,
            Role         = request.Role,
            IsActive     = true,
            CreatedAt    = DateTime.UtcNow
        };

        await _uow.Users.AddAsync(user);
        await _uow.SaveChangesAsync();
        return ToResponse(user);
    }

    public async Task<UserResponse> UpdateAsync(int id, CreateUserRequest request)
    {
        var user = await _uow.Users.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Usuario con Id {id} no encontrado.");

        user.Name  = request.Name;
        user.Email = request.Email;
        user.Role  = request.Role;

        if (!string.IsNullOrWhiteSpace(request.Password))
            user.PasswordHash = _hasher.Hash(request.Password);

        _uow.Users.Update(user);
        await _uow.SaveChangesAsync();
        return ToResponse(user);
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _uow.Users.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Usuario con Id {id} no encontrado.");

        user.IsActive = false;   // soft delete
        _uow.Users.Update(user);
        await _uow.SaveChangesAsync();
    }

    private static UserResponse ToResponse(User u) => new()
    {
        Id        = u.Id,
        Username  = u.Username,
        Name      = u.Name,
        Email     = u.Email,
        Role      = u.Role,
        IsActive  = u.IsActive,
        CreatedAt = u.CreatedAt
    };
}
