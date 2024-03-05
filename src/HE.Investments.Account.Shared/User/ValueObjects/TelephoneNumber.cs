using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investments.Account.Shared.User.ValueObjects;

public class TelephoneNumber : ValueObject
{
    public TelephoneNumber(string value)
    {
        Value = value;
    }

    private TelephoneNumber(string? telephoneNumber, string fieldName, string fieldLabel)
    {
        Build(telephoneNumber, fieldName, fieldLabel).CheckErrors();
    }

    public string Value { get; private set; }

    public static TelephoneNumber FromString(string? telephoneNumber, string fieldName, string fieldLabel) =>
        new(telephoneNumber, fieldName, fieldLabel);

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    private OperationResult Build(string? telephoneNumber, string fieldName, string fieldLabel)
    {
        var operationResult = OperationResult.New();

        var value = TelephoneNumberValidator
            .For(telephoneNumber, fieldName, fieldLabel, operationResult)
            .IsValid();

        if (value.IsProvided())
        {
            Value = value!;
        }

        return operationResult;
    }
}
