using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.BusinessLogic.Projects.Consts;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;

namespace HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
public class Coordinates : ValueObject
{
    public Coordinates(string value)
    {
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(ProjectValidationFieldNames.Coordinates, ValidationErrorMessage.EnterCoordinates)
                .CheckErrors();
        }

        if (value!.Length > MaximumInputLength.LongInput)
        {
            OperationResult.New()
                .AddValidationError(ProjectValidationFieldNames.Coordinates, ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.LocationCoordinates))
                .CheckErrors();
        }

        Value = value;
    }

    public string Value { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
