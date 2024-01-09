using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Common.ValueObjects;

public class FileName : ValueObject
{
    public FileName(string value)
    {
        if (ObjectExtensions.IsNotProvided(value))
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
