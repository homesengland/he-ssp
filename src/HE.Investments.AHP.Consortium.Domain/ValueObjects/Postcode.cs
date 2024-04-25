using System.Text.RegularExpressions;
using HE.Investments.Common;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Messages;

namespace HE.Investments.AHP.Consortium.Domain.ValueObjects;

public class Postcode : ShortText
{
    public Postcode(string? value)
        : base(value, nameof(Postcode), "organisation postcode")
    {
        var match = Regex.Match(Value, Constants.UkPostcodeRegex, RegexOptions.IgnoreCase);
        if (!match.Success)
        {
            OperationResult.ThrowValidationError(nameof(Postcode), OrganisationErrorMessages.InvalidOrganisationPostcode);
        }
    }
}
