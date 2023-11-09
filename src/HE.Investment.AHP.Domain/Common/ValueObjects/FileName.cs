using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Validation;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Common.ValueObjects;

public class FileName : ValueObject
{
    public FileName(string value)
    {
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(nameof(FileName), GenericValidationError.NoValueProvided)
                .CheckErrors();
        }

        Value = value;
    }

    public string Value { get; }

    public FileExtension Extension => new(Value.Split('.').Skip(1).LastOrDefault());

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
