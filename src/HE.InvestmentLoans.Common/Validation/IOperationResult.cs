namespace HE.InvestmentLoans.Common.Validation;

public interface IOperationResult
{
    OperationResult AddValidationErrors(IList<ErrorItem> errorItem);
}
