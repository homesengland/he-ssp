using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Utils;

namespace HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;

public class PurchaseDate : DateValueObject
{
    private const string FieldDescription = "purchase date";

    public PurchaseDate(string? day, string? month, string? year, IDateTimeProvider dateTimeProvider)
        : base(day, month, year, nameof(PurchaseDate), FieldDescription)
    {
        if (Value.IsAfter(dateTimeProvider.Now.Date))
        {
            OperationResult.ThrowValidationError(nameof(PurchaseDate), ValidationErrorMessage.FuturePurchaseDate);
        }
    }

    private PurchaseDate(DateTime value)
        : base(value)
    {
    }

    public static PurchaseDate FromCrm(DateTime value) => new(value);

    public static PurchaseDate FromDateDetails(DateDetails? date, IDateTimeProvider dateTimeProvider) =>
        new(date?.Day, date?.Month, date?.Year, dateTimeProvider);
}
