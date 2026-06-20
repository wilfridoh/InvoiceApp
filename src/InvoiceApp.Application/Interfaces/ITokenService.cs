using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
    DateTime GetExpiry();
}
