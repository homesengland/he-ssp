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
