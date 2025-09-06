using FluentValidation.Results;

namespace TaskManager.Application.Common.Exceptions;

public class ValidationRequestException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationRequestException()
        : base("Ocorreram uma ou mais falhas de validação.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationRequestException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }
}