using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Validators;

namespace HE.Investments.Account.Shared.User.ValueObjects;

public class TelephoneNumber : ValueObject
{
    public TelephoneNumber(string? telephoneNumber, string fieldName, string fieldLabel, bool isOptional = false)
    {
        Build(telephoneNumber, fieldName, fieldLabel, isOptional).CheckErrors();
    }

    public TelephoneNumber(string? value)
    {
        Value = value;
    }

    public string? Value { get; private set; }

    public override string ToString()
    {
        return Value ?? string.Empty;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    private OperationResult Build(string? telephoneNumber, string fieldName, string fieldLabel, bool isOptional)
    {
        var operationResult = OperationResult.New();

        Value = TelephoneNumberValidator
            .For(telephoneNumber, fieldName, fieldLabel, isOptional, operationResult)
            .IsValid();

        return operationResult;
    }
}
