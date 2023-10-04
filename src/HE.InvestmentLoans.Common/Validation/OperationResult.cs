using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Extensions;

namespace HE.InvestmentLoans.Common.Validation;

public class OperationResult
{
    public IList<ErrorItem> Errors { get; } = new List<ErrorItem>();

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

    public TReturnedData CatchResult<TReturnedData>(Func<TReturnedData> action, string overriddenFieldName = null!)
        where TReturnedData : class
    {
        try
        {
            return action();
        }
        catch (DomainValidationException ex)
        {
            var result = ex.OperationResult;
            var error = result.Errors.Single();

            AddValidationError(overriddenFieldName.IsProvided() ? overriddenFieldName : error.AffectedField, error.ErrorMessage);

            return null!;
        }
    }
}
