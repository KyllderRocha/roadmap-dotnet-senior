using FluentValidation;

public class DeleteTaskCommandValidator : AbstractValidator<DeleteTaskCommand>
{
    public DeleteTaskCommandValidator()
    {
        RuleFor(x => x.id)
            .NotEmpty().WithMessage("O ID é obrigatório.");
    }
}