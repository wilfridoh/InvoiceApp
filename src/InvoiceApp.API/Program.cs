using FluentValidation;
using FluentValidation.AspNetCore;
using InvoiceApp.API.Middleware;
using InvoiceApp.Infrastructure;
using InvoiceApp.Infrastructure.Data;

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

// ── CORS ─────────────────────────────────────────────────────
builder.Services.AddCors(opts =>
    opts.AddPolicy("FrontendPolicy", p =>
        p.WithOrigins("http://localhost:4200")
         .AllowAnyHeader()
         .AllowAnyMethod()));

// ─────────────────────────────────────────────────────────────
var app = builder.Build();

// ── Seed inicial ─────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DataSeeder.Seed(db);
}

// ── Pipeline ─────────────────────────────────────────────────
app.UseMiddleware<ExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");
app.UseAuthorization();
app.MapControllers();

app.Run();
