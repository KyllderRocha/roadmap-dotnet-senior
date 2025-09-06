using FluentValidation;

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O ID é obrigatório.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("O título é obrigatório.")
            .MaximumLength(200).WithMessage("O título não pode exceder 200 caracteres.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("O ID do usuário é obrigatório.");
    }
}