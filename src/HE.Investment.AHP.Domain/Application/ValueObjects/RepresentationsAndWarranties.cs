using HE.Investment.AHP.Domain.Application.Constants;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public class RepresentationsAndWarranties : ValueObject
{
    public RepresentationsAndWarranties(bool value)
    {
        Value = value;
    }

    public bool Value { get; }

    public static RepresentationsAndWarranties FromString(string? value)
    {
        if (value != "checked")
        {
            OperationResult.New()
                .AddValidationError(nameof(RepresentationsAndWarranties), ApplicationValidationErrors.MissingConfirmation)
                .CheckErrors();
        }

        return new(true);
    }

    public static RepresentationsAndWarranties Confirmed() => new(true);

    public override string ToString()
    {
        return Value.ToString();
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
