using System.Xml.Linq;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Constants;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Site.Entities;

public class Section106Entity : IQuestion
{
    public Section106Entity(
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

        this.GeneralAgreement = agreement!.Value;
        this.AffordableHousing = affordableHousing;
        this.OnlyAffordableHousing = onlyAffordableHousing;
        this.AdditionalAffordableHousing = additionalAffordableHousing;
        this.CapitalFundingEligibility = capitalFundingEligibility;
        this.ConfirmationFromLocalAuthority = confirmationFromLocalAuthority;
    }

    public Section106Entity()
    {
    }

    public bool? GeneralAgreement { get; private set; }

    public bool? AffordableHousing { get; private set; }

    public bool? OnlyAffordableHousing { get; private set; }

    public bool? AdditionalAffordableHousing { get; private set; }

    public bool? CapitalFundingEligibility { get; private set; }

    public string? ConfirmationFromLocalAuthority { get; private set; }

    public void ProvideGeneralAgreement(bool? generalAgreement)
    {
        if (generalAgreement is null)
        {
            OperationResult.New()
                .AddValidationError(SiteValidationFieldNames.Section106Agreement, ValidationErrorMessage.MissingRequiredField("Section 106 Agreement"))
                .CheckErrors();
        }

        this.GeneralAgreement = generalAgreement!.Value;
    }

    public void ProvideAffordableHousing(bool? affordableHousing)
    {
        this.AffordableHousing = affordableHousing;
    }

    public void ProvideOnlyAffordableHousing(bool? onlyAffordableHousing)
    {
        this.OnlyAffordableHousing = onlyAffordableHousing;
    }

    public void ProvideAdditionalAffrdableHousing(bool? additionalAffordableHousing)
    {
        this.AdditionalAffordableHousing = additionalAffordableHousing;
    }

    public void ProvideCapitalFundingEligibility(bool? capitalFundingEligibility)
    {
        this.CapitalFundingEligibility = capitalFundingEligibility;
    }

    public void ProvideConfirmationFromLocalAuthority(string? confirmationFromLocalAuthority)
    {
        this.ConfirmationFromLocalAuthority = confirmationFromLocalAuthority;
    }

    public bool IsAnswered()
    {
        if (this.GeneralAgreement != null)
        {
            return true;
        }

        return false;
    }
}
