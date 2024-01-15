using System.Runtime.CompilerServices;
using HE.Investment.AHP.Contract.Site.Constants;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Site.ValueObjects;

public class SiteSection106Agreement : ValueObject
{
    public SiteSection106Agreement(bool? value)
    {
        if (value == null)
        {
            OperationResult.New().
                AddValidationError(
                    SiteValidationFieldNames.Section106Agreement,
                    ValidationErrorMessage.MissingRequiredField(SiteValidationFieldNames.Section106Agreement)).CheckErrors();
        }
        else
        {
            this.Value = value.Value;
        }
    }

    public bool Value { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
