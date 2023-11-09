using HE.Investments.Common.Validators;

namespace HE.Investments.Common.Exceptions;

public class DomainValidationException : Exception
{
    public DomainValidationException(OperationResult operationResult)
        : base(operationResult.GetAllErrors())
    {
        OperationResult = operationResult;
    }

    public OperationResult OperationResult { get; }
}
