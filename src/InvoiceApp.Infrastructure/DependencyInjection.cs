using InvoiceApp.Application.Interfaces;
using InvoiceApp.Application.Services;
using InvoiceApp.Domain.Interfaces;
using InvoiceApp.Infrastructure.Data;
using InvoiceApp.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var rawConnectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection no configurado.");

        services.AddDbContext<AppDbContext>(opts =>
            opts.UseNpgsql(rawConnectionString));

        services.AddScoped<IUnitOfWork,     UnitOfWork>();
        services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
        services.AddScoped<IUserService,    UserService>();
        services.AddScoped<IAuthService,    AuthService>();
        services.AddScoped<ITokenService,   JwtTokenService>();
        services.AddScoped<IClientService,  ClientService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IInvoiceDetailService, InvoiceDetailService>();
        services.AddScoped<IInvoicePaymentService, InvoicePaymentService>();
        services.AddScoped<IPaymentMethodService, PaymentMethodService>();

        return services;
    }
}
