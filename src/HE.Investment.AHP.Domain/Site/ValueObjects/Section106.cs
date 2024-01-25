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
        string? localAuthorityConfirmation)
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
        LocalAuthorityConfirmation = localAuthorityConfirmation;
    }

    public Section106()
    {
    }

    public bool? GeneralAgreement { get; }

    public bool? AffordableHousing { get; }

    public bool? OnlyAffordableHousing { get; }

    public bool? AdditionalAffordableHousing { get; }

    public bool? CapitalFundingEligibility { get; }

    public string? LocalAuthorityConfirmation { get; }

    public bool IsAnswered()
    {
        if (GeneralAgreement == false)
        {
            return true;
        }

        if (GeneralAgreement == true)
        {
            if (AffordableHousing == false)
            {
                return CapitalFundingEligibility != null;
            }

            if (AffordableHousing == true)
            {
                if (OnlyAffordableHousing == true)
                {
                    return CapitalFundingEligibility != null;
                }

                if (OnlyAffordableHousing == false)
                {
                    return AdditionalAffordableHousing != null && CapitalFundingEligibility != null;
                }
            }
        }

        return false;
    }

    public bool IsIneligibleDueToAffordableHousing()
    {
        return AffordableHousing == true && OnlyAffordableHousing == false && AdditionalAffordableHousing == false;
    }

    public bool IsIneligibleDueToCapitalFundingGuide()
    {
        return CapitalFundingEligibility == true;
    }

    public bool IsIneligible()
    {
        return IsIneligibleDueToAffordableHousing() || IsIneligibleDueToCapitalFundingGuide();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return GeneralAgreement;
        yield return AffordableHousing;
        yield return OnlyAffordableHousing;
        yield return AdditionalAffordableHousing;
        yield return CapitalFundingEligibility;
        yield return LocalAuthorityConfirmation;
    }
}
