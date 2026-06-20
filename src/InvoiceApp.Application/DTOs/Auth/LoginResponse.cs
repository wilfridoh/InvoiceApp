namespace InvoiceApp.Application.DTOs.Auth;

public record LoginResponse(
    string Token,
    string ExpiresAt,
    string Username,
    string Role);
