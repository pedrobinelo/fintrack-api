using FinTrackAPI.DTOs;
using FluentValidation;

namespace FinTrackAPI.Validators
{
    public class LoginDTOValidator : AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .EmailAddress().WithMessage("Informe um e-mail válido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("A senha é obrigatória.");
        }
    }
}