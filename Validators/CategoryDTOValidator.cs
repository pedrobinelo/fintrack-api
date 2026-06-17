using FinTrackAPI.DTOs;
using FluentValidation;

namespace FinTrackAPI.Validators
{
    public class CategoryDTOValidator : AbstractValidator<CategoryDTO>
    {
        public CategoryDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome da categoria é obrigatório.")
                .MaximumLength(50).WithMessage("O nome da categoria deve ter no máximo 50 caracteres.");
        }
    }
}
