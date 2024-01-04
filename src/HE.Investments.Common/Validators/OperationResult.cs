using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Extensions;

namespace HE.Investments.Common.Validators;

public class OperationResult : IOperationResult
{
    public OperationResult()
    {
        Errors = new List<ErrorItem>();
    }

    public OperationResult(IEnumerable<ErrorItem>? errors = null)
    {
        Errors = errors?.ToList() ?? new List<ErrorItem>();
    }

    public IList<ErrorItem> Errors { get; protected set; }

    public bool IsValid => Errors.Count == 0;

    public bool HasValidationErrors => !IsValid;

    public static OperationResult New() => new();

    public static OperationResult Success() => new();

    public static OperationResult<TResult> Success<TResult>(TResult result) => new(result);

    public static void ThrowValidationError(string affectedField, string validationMessage) =>
        New().AddValidationError(affectedField, validationMessage).CheckErrors();

    public static OperationResult<TReturnedData> ResultOf<TReturnedData>(Func<TReturnedData> action)
        where TReturnedData : class
    {
        var operationResult = New();

        var returnedData = operationResult.CatchResult(action);

        return operationResult.Returns(returnedData);
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

    // This is not true that method returns not nullable object in case of error.
    // But this is useful for validation errors aggregation for nested value objects.
    // Child can be nullable, but we still need aggregated errors for parent.
    public TReturnedData Aggregate<TReturnedData>(Func<TReturnedData> action)
        where TReturnedData : class
    {
        try
        {
            return action();
        }
        catch (DomainValidationException ex)
        {
            var result = ex.OperationResult;

            AddValidationErrors(result.Errors);

            return null!;
        }
    }

    public TReturnedData? AggregateNullable<TReturnedData>(Func<TReturnedData?> action)
        where TReturnedData : class
    {
        try
        {
            return action();
        }
        catch (DomainValidationException ex)
        {
            var result = ex.OperationResult;

            AddValidationErrors(result.Errors);

            return null;
        }
    }

    public void Aggregate(Action action)
    {
        try
        {
            action();
        }
        catch (DomainValidationException ex)
        {
            var result = ex.OperationResult;

            AddValidationErrors(result.Errors);
        }
    }

    public OperationResult WithValidation(Action action)
    {
        try
        {
            action();
        }
        catch (DomainValidationException ex)
        {
            var result = ex.OperationResult;
            var error = result.Errors.Single();

            AddValidationError(error.AffectedField, error.ErrorMessage);
        }

        return this;
    }

    public OperationResult AddValidationError(ErrorItem errorItem)
    {
        Errors.Add(errorItem);
        return this;
    }

    public OperationResult AddValidationErrors(IList<ErrorItem> errorItem)
    {
        Errors.AddRange(errorItem);
        return this;
    }

    public OperationResult AddValidationError(string affectedField, string validationMessage)
    {
        return AddValidationError(new ErrorItem(affectedField, validationMessage));
    }

    public OperationResult<TResult> Returns<TResult>(TResult returnedObject)
    {
        return new OperationResult<TResult>(Errors, returnedObject);
    }

    public OperationResult<TResult> AsGeneric<TResult>()
    {
        return new OperationResult<TResult>(Errors, default!);
    }

    public OperationResult OverrideError(string message, string overriddenAffectedField, string overriddenValidationMessage)
    {
        var error = Errors.FirstOrDefault(c => c.ErrorMessage == message);

        if (error is null)
        {
            return this;
        }

        Errors.Remove(error);

        Errors.Add(new ErrorItem(overriddenAffectedField, overriddenValidationMessage));

        return this;
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
