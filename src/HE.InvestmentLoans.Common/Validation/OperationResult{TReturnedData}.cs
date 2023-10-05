using HE.InvestmentLoans.Common.Exceptions;

namespace HE.InvestmentLoans.Common.Validation;

public class OperationResult<TReturnedData> : OperationResult
{
    public OperationResult(TReturnedData value)
        : base()
    {
        ReturnedData = value;
    }

    public OperationResult(IEnumerable<ErrorItem> errorItems, TReturnedData value)
    {
        ReturnedData = value;
        Errors = errorItems.ToList();
    }

    public TReturnedData ReturnedData { get; private set; }
}
