using System.Text.Json.Serialization;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;

namespace HE.Investments.Account.Shared.User.ValueObjects;

public class OrganisationId : ValueObject
{
    [JsonConstructor]
    public OrganisationId(Guid value)
    {
        Value = value;
    }

    public OrganisationId(string? value)
    {
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(nameof(OrganisationId), GenericValidationError.NoValueProvided)
                .CheckErrors();
        }

        if (!Guid.TryParse(value, out var guid))
        {
            OperationResult.New()
                .AddValidationError(nameof(OrganisationId), "Invalid OrganisationId")
                .CheckErrors();
        }

        Value = guid;
    }

    public Guid Value { get; }

    public override string ToString()
    {
        return Value.ToString();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
