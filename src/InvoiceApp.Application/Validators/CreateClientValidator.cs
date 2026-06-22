using FluentValidation;
using InvoiceApp.Application.DTOs.Clients;

namespace InvoiceApp.Application.Validators;

public class CreateClientValidator : AbstractValidator<CreateClientRequest>
{
    private static readonly string[] ValidDocTypes = ["RUC", "CI", "Pasaporte"];

    public CreateClientValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(200).WithMessage("El nombre no puede superar 200 caracteres.");

        RuleFor(x => x.DocumentType)
            .NotEmpty().WithMessage("El tipo de documento es requerido.")
            .Must(t => ValidDocTypes.Contains(t))
            .WithMessage("El tipo de documento debe ser: RUC, CI o Pasaporte.");

        RuleFor(x => x.DocumentNumber)
            .NotEmpty().WithMessage("El número de documento es requerido.")
            .MaximumLength(20).WithMessage("El número de documento no puede superar 20 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido.")
            .EmailAddress().WithMessage("El email no tiene un formato válido.")
            .MaximumLength(150).WithMessage("El email no puede superar 150 caracteres.");

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("El teléfono no puede superar 20 caracteres.");

        RuleFor(x => x.Address)
            .MaximumLength(300).WithMessage("La dirección no puede superar 300 caracteres.");
    }
}
