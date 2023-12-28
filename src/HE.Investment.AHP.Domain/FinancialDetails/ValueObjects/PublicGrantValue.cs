using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

public class PublicGrantValue : PoundsValueObject
{
    public PublicGrantValue(PublicGrantFields field, decimal value)
        : base(value, new UiFields(field.ToString()))
    {
        Field = field;
    }

    public PublicGrantValue(PublicGrantFields field, string value)
        : base(value, new UiFields(field.ToString()))
    {
        Field = field;
    }

    public PublicGrantFields Field { get; }
}
