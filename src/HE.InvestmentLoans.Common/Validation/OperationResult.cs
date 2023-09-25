using HE.InvestmentLoans.Common.Exceptions;

namespace HE.InvestmentLoans.Common.Validation;

public class OperationResult<TResult> : OperationResult
{
    public OperationResult(TResult value)
    {
        Result = value;
    }

    public TResult Result { get; private set; }
}

public class OperationResult
{
    public OperationResult()
    {
        Errors = new List<ErrorItem>();
    }

    public IList<ErrorItem> Errors { get; }

    public bool IsValid => Errors.Count == 0;

    public bool HasValidationErrors => !IsValid;

    public static OperationResult New() => new();

    public static OperationResult Success() => new();

    public static OperationResult<TResult> Success<TResult>(TResult result) => new(result);

    public OperationResult AddValidationError(ErrorItem errorItem)
    {
        Errors.Add(errorItem);
        return this;
    }

    public OperationResult AddValidationError(string affectedField, string validationMessage)
    {
        return AddValidationError(new ErrorItem(affectedField, validationMessage));
    }

    public string GetAllErrors()
    {
        return string.Join(Environment.NewLine, Errors.Select(x => x.ErrorMessage));
    }

    public void CheckErrors()
    {
        if (HasValidationErrors)
        {
            throw new DomainValidationException(this);
        }
    }
}
