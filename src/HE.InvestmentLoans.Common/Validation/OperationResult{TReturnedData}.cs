namespace HE.InvestmentLoans.Common.Validation;

public class OperationResult<TReturnedData> : OperationResult
{
    public OperationResult()
    {
        Errors = new List<ErrorItem>();
    }

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
