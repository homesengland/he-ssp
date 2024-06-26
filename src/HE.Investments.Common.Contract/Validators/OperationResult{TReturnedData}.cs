namespace HE.Investments.Common.Contract.Validators;

public class OperationResult<TReturnedData> : OperationResult
{
    public OperationResult()
    {
        Errors = [];
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
