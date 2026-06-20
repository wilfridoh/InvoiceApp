using InvoiceApp.Application.DTOs.Auth;

namespace InvoiceApp.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
}
