using System.Text.RegularExpressions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investments.AHP.Consortium.Domain.ValueObjects;

public partial class Postcode : ShortText
{
    public Postcode(string? value)
        : base(value, nameof(Postcode), "organisation postcode")
    {
        var match = PostcodeRegex().Match(Value);
        if (!match.Success)
        {
            OperationResult.ThrowValidationError(nameof(Postcode), OrganisationErrorMessages.InvalidOrganisationPostcode);
        }
    }

    [GeneratedRegex(Constants.UkPostcodeRegex, RegexOptions.IgnoreCase, "pl-PL")]
    private static partial Regex PostcodeRegex();
}
