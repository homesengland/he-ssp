namespace HE.Investments.Common.Contract.Validators;

public interface IOperationResult
{
    OperationResult AddValidationErrors(IList<ErrorItem> errorItem);
}
