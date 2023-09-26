using HE.InvestmentLoans.Common.Validation;

namespace HE.InvestmentLoans.Common.Exceptions;

public class DomainValidationException : Exception
{
    public DomainValidationException(OperationResult operationResult)
        : base(operationResult.GetAllErrors())
    {
        OperationResult = operationResult;
    }

    public OperationResult OperationResult { get; }
}
