using InvoiceApp.Application.DTOs.Auth;
using InvoiceApp.Application.Interfaces;
using InvoiceApp.Domain.Interfaces;

namespace InvoiceApp.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokenService;

    public AuthService(IUnitOfWork uow, IPasswordHasher hasher, ITokenService tokenService)
    {
        _uow = uow;
        _hasher = hasher;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _uow.Users.GetByUsernameAsync(request.Username)
            ?? throw new UnauthorizedAccessException("Usuario o contraseña incorrectos.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Usuario inactivo. Contacte al administrador.");

        if (!_hasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Usuario o contraseña incorrectos.");

        var token = _tokenService.GenerateToken(user);
        var expiry = _tokenService.GetExpiry();

        return new LoginResponse(token, expiry.ToString("o"), user.Username, user.Role);
    }
}
