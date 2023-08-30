using HE.InvestmentLoans.Common.Exceptions;

namespace HE.InvestmentLoans.Common.Validation;

public class OperationResult
{
    public OperationResult()
    {
        Errors = new List<ErrorItem>();
    }

    public IList<ErrorItem> Errors { get; }

    public bool IsValid => Errors.Count == 0;

    public bool AreValidationErrors => !IsValid;

    public static OperationResult New() => new();

    public static OperationResult Success() => new();

    public OperationResult AddValidationError(ErrorItem errorItem)
    {
        Errors.Add(errorItem);
        return this;
    }

    public OperationResult AddValidationError(string affectedField, string validationMessage)
    {
        return AddValidationError(new ErrorItem(affectedField, validationMessage));
    }

    public void ThrowException()
    {
        if (AreValidationErrors)
        {
            throw new DomainValidationException(this);
        }
    }
}
