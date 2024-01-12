using HE.Investments.Common.Contract.Validators;

namespace HE.Investments.Common.Contract.Exceptions;

public class DomainValidationException : Exception
{
    public DomainValidationException(OperationResult operationResult)
        : base(operationResult.GetAllErrors())
    {
        OperationResult = operationResult;
    }

    public DomainValidationException(string errorMessage)
        : this(string.Empty, errorMessage)
    {
    }

    public DomainValidationException(string fieldName, string errorMessage)
        : base(OperationResult.New().AddValidationError(fieldName, errorMessage).GetAllErrors())
    {
        OperationResult = OperationResult.New();
        OperationResult.AddValidationError(fieldName, errorMessage);
    }

    public OperationResult OperationResult { get; }
}
