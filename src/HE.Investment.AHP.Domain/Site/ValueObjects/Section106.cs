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
        bool? affordableHousing = null,
        bool? onlyAffordableHousing = null,
        bool? additionalAffordableHousing = null,
        bool? capitalFundingEligibility = null,
        string? localAuthorityConfirmation = null)
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

    public Section106 WithGeneralAgreement(bool? generalAgreement)
    {
        var result = this;
        if (GeneralAgreement != generalAgreement || generalAgreement == null)
        {
            result = new Section106(generalAgreement);
        }

        return result;
    }

    public Section106 WithAffordableHousing(bool? affordableHousing)
    {
        if (affordableHousing != AffordableHousing)
        {
            return new Section106(GeneralAgreement, affordableHousing);
        }

        return this;
    }

    public Section106 WithOnlyAffordableHousing(bool? onlyAffordableHousing)
    {
        if (onlyAffordableHousing != OnlyAffordableHousing)
        {
            return new Section106(GeneralAgreement, AffordableHousing, onlyAffordableHousing);
        }

        return this;
    }

    public Section106 WithAdditionalAffordableHousing(bool? additionalAffordableHousing)
    {
        if (AdditionalAffordableHousing != additionalAffordableHousing)
        {
            return new Section106(GeneralAgreement, AffordableHousing, OnlyAffordableHousing, additionalAffordableHousing);
        }

        return this;
    }

    public Section106 WithCapitalFundingEligibility(bool? capitalFundingEligibility)
    {
        if (CapitalFundingEligibility != capitalFundingEligibility)
        {
            return new Section106(GeneralAgreement, AffordableHousing, OnlyAffordableHousing, AdditionalAffordableHousing, capitalFundingEligibility);
        }

        return this;
    }

    public Section106 WithLocalAuthorityConfirmation(string? localAuthorityConfirmation)
    {
        return new Section106(GeneralAgreement, AffordableHousing, OnlyAffordableHousing, AdditionalAffordableHousing, CapitalFundingEligibility, localAuthorityConfirmation);
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
