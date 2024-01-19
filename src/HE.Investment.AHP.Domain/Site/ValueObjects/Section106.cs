using System.Xml.Linq;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Constants;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Site.ValueObjects;

public class Section106 : ValueObject, IQuestion
{
    public Section106(
        bool? agreement,
        bool? affordableHousing,
        bool? onlyAffordableHousing,
        bool? additionalAffordableHousing,
        bool? capitalFundingEligibility,
        string? confirmationFromLocalAuthority)
    {
        if (agreement == null)
        {
            OperationResult.New()
                .AddValidationError(SiteValidationFieldNames.Section106Agreement, ValidationErrorMessage.MissingRequiredField(SiteValidationFieldNames.Section106Agreement))
                .CheckErrors();
        }

        GeneralAgreement = agreement!.Value;
        AffordableHousing = affordableHousing;
        OnlyAffordableHousing = onlyAffordableHousing;
        AdditionalAffordableHousing = additionalAffordableHousing;
        CapitalFundingEligibility = capitalFundingEligibility;
        ConfirmationFromLocalAuthority = confirmationFromLocalAuthority;
    }

    public Section106()
    {
    }

    public bool? GeneralAgreement { get; }

    public bool? AffordableHousing { get; }

    public bool? OnlyAffordableHousing { get; }

    public bool? AdditionalAffordableHousing { get; }

    public bool? CapitalFundingEligibility { get; }

    public string? ConfirmationFromLocalAuthority { get; }

    public bool IsAnswered()
    {
        if (GeneralAgreement != null)
        {
            return true;
        }

        return false;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return GeneralAgreement;
        yield return AffordableHousing;
        yield return OnlyAffordableHousing;
        yield return AdditionalAffordableHousing;
        yield return CapitalFundingEligibility;
        yield return ConfirmationFromLocalAuthority;
    }
}
