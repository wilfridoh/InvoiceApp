using InvoiceApp.Application.DTOs.Common;
using InvoiceApp.Application.DTOs.Users;

namespace InvoiceApp.Application.Interfaces;

public interface IUserService
{
    Task<PagedResult<UserResponse>> GetUsersAsync(int page = 1, int pageSize = 20);
    Task<UserResponse?>             GetByIdAsync(int id);
    Task<UserResponse>              CreateAsync(CreateUserRequest request);
    Task<UserResponse>              UpdateAsync(int id, CreateUserRequest request);
    Task                            DeleteAsync(int id);
}
