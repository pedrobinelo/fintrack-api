using FinTrackAPI.DTOs;
using FluentValidation;

namespace FinTrackAPI.Validators
{
    public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MinimumLength(2).WithMessage("O nome deve ter pelo menos 2 caracteres.")
                .Matches(@"^[A-Za-zÀ-ÿ\s]+$").WithMessage("O nome deve conter apenas letras.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .EmailAddress().WithMessage("O e-mail informado não é válido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.");
        }
    }
}
