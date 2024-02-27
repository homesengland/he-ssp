using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.ValueObjects;

public class InformationAgreement : ValueObject
{
    public InformationAgreement(string? value)
    {
        if (bool.TryParse(value, out var parsedValue) != true)
        {
            OperationResult
                .New()
                .AddValidationError(nameof(InformationAgreement), ValidationErrorMessage.InformationAgreement)
                .CheckErrors();
        }

        Value = parsedValue;
    }

    public bool Value { get; }

    public static InformationAgreement FromString(string? value) => new(value);


    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
