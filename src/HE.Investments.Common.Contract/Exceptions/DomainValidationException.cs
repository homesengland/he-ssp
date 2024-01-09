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
        : base(OperationResult.New().AddValidationError(string.Empty, errorMessage).GetAllErrors())
    {
        OperationResult = OperationResult.New();
        OperationResult.AddValidationError(string.Empty, errorMessage);
    }

    public OperationResult OperationResult { get; }
}
