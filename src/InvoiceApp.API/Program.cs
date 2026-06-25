using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using InvoiceApp.API.Middleware;
using InvoiceApp.Infrastructure;
using InvoiceApp.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// ── Controllers ────────────────────────────────────────────
builder.Services.AddControllers();

// ── Swagger ─────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "InvoiceApp API", Version = "v1" });
});

// ── FluentValidation ────────────────────────────────────────
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<
    InvoiceApp.Application.Validators.CreateUserValidator>();

// ── Infrastructure (DbContext + Repos + Services) ───────────
builder.Services.AddInfrastructure(builder.Configuration);

// ── JWT Authentication ─────────────────────────────────────
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var jwtSecret = jwtSettings["Secret"]!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer           = true,
        ValidateAudience         = true,
        ValidateLifetime         = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer              = jwtSettings["Issuer"],
        ValidAudience            = jwtSettings["Audience"],
        IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
    };
});

// ── CORS ─────────────────────────────────────────────────────
builder.Services.AddCors(opts =>
    opts.AddPolicy("FrontendPolicy", p =>
        p.WithOrigins("http://localhost:4200")
         .AllowAnyHeader()
         .AllowAnyMethod()));

// ─────────────────────────────────────────────────────────────
var app = builder.Build();

// ── Seed inicial (opcional) ──────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Run seeding only if DataSeeder exists in the deployed assembly.
    var seederType = Type.GetType("InvoiceApp.Infrastructure.Data.DataSeeder, InvoiceApp.Infrastructure");
    var seedMethod = seederType?.GetMethod("Seed", new[] { typeof(AppDbContext) });
    seedMethod?.Invoke(null, new object[] { db });
}

// ── Pipeline ─────────────────────────────────────────────────
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "swagger";
});

app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapGet("/health/db", async (AppDbContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();

    return canConnect
        ? Results.Ok(new { success = true, message = "Database connection OK" })
        : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
});

app.MapGet("/health/db-config", (IConfiguration config) =>
{
    var raw = config.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(raw))
        return Results.Problem("ConnectionStrings:DefaultConnection no configurado.", statusCode: 500);

    var sanitized = raw.Trim().Trim('"', '\'');
    var builder = new NpgsqlConnectionStringBuilder(sanitized);

    return Results.Ok(new
    {
        success = true,
        host = builder.Host,
        database = builder.Database,
        port = builder.Port,
        sslMode = builder.SslMode.ToString(),
        hasUser = !string.IsNullOrWhiteSpace(builder.Username)
    });
});

app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
