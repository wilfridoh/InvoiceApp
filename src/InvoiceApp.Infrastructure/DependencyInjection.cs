using InvoiceApp.Application.Interfaces;
using InvoiceApp.Application.Services;
using InvoiceApp.Domain.Interfaces;
using InvoiceApp.Infrastructure.Data;
using InvoiceApp.Infrastructure.Services;
using Microsoft.Data.SqlClient;
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

        var normalizedConnectionString = NormalizeSqlConnectionString(rawConnectionString);

        services.AddDbContext<AppDbContext>(opts =>
            opts.UseSqlServer(normalizedConnectionString));

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

    private static string NormalizeSqlConnectionString(string value)
    {
        var sanitized = value.Trim().Trim('"', '\'');
        var builder = new SqlConnectionStringBuilder(sanitized);

        if (builder.DataSource.StartsWith("tcp:", StringComparison.OrdinalIgnoreCase))
            builder.DataSource = builder.DataSource[4..];

        return builder.ConnectionString;
    }
}
