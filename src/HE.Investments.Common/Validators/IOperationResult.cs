namespace HE.Investments.Common.Validators;

public interface IOperationResult
{
    OperationResult AddValidationErrors(IList<ErrorItem> errorItem);
}
