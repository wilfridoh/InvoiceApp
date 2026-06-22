using FluentValidation;
using InvoiceApp.Application.DTOs.Products;

namespace InvoiceApp.Application.Validators;

public class UpdateProductValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("El código es requerido.")
            .MaximumLength(50).WithMessage("El código no puede superar 50 caracteres.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(200).WithMessage("El nombre no puede superar 200 caracteres.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("La descripción no puede superar 500 caracteres.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo.");
    }
}
