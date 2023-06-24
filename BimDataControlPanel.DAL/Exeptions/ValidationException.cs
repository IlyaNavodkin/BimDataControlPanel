using FluentValidation.Results;

namespace BimDataControlPanel.DAL.Exeptions;

public class ValidationException : Exception
{
    public List<ValidationFailure> Errors;

    public ValidationException(List<ValidationFailure> errors )
        : base($"Validation error")
    {
        Errors = errors;
    }
}