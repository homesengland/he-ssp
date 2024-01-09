using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.BusinessLogic.Projects.Consts;

namespace HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
public class PurchaseDate
{
    public PurchaseDate(ProjectDate date, DateTime now)
    {
        if (date.Value.IsAfter(now))
        {
            OperationResult.ThrowValidationError(ProjectValidationFieldNames.PurchaseDay, ValidationErrorMessage.FuturePurchaseDate);
        }

        Date = date;
    }

    public ProjectDate Date { get; }

    public static PurchaseDate FromString(string year, string month, string day, DateTime now)
    {
        var operationResult = OperationResult.ResultOf(() => ProjectDate.FromString(year, month, day));

        operationResult.OverrideError(GenericValidationError.NoDate, ProjectValidationFieldNames.PurchaseDate, ValidationErrorMessage.NoPurchaseDate);
        operationResult.OverrideError(GenericValidationError.InvalidDate, ProjectValidationFieldNames.PurchaseDate, ValidationErrorMessage.IncorrectPurchaseDate);

        operationResult.CheckErrors();

        return new PurchaseDate(operationResult.ReturnedData, now);
    }

    internal DateTime AsDateTime()
    {
        return Date.Value;
    }
}
