using FluentValidation;
using FinTrack_API.DTOs;

namespace FinTrack_API.Validators
{
    public class TransactionDTOValidator : AbstractValidator<TransactionDTO> 
    {
        public TransactionDTOValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("A descrição é obrigatória.")
                .MaximumLength(100).WithMessage("A descrição deve ter no máximo 100 caracteres.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("O valor da transação deve ser maior que zero.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("A data da transação é obrigatória.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Uma categoria válida deve ser informada.");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("O tipo de transação deve ser 0 (Receita) ou 1 (Despesa).");
        }
    }
}
