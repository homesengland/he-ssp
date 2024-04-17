using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
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
                .AddValidationError(nameof(Section106Dto.GeneralAgreement), ValidationErrorMessage.MustProvideRequiredField("General Agreement"))
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
        if (affordableHousing == null)
        {
            OperationResult.New()
                .AddValidationError(nameof(Section106Dto.AffordableHousing), ValidationErrorMessage.MustProvideRequiredField("Affordable Housing answer"))
                .CheckErrors();
        }

        if (affordableHousing != AffordableHousing)
        {
            return new Section106(GeneralAgreement, affordableHousing);
        }

        return this;
    }

    public Section106 WithOnlyAffordableHousing(bool? onlyAffordableHousing)
    {
        if (onlyAffordableHousing == null)
        {
            OperationResult.New()
                .AddValidationError(nameof(Section106Dto.OnlyAffordableHousing), ValidationErrorMessage.MustProvideRequiredField("100% Affordable Housing answer"))
                .CheckErrors();
        }

        if (onlyAffordableHousing != OnlyAffordableHousing)
        {
            return new Section106(GeneralAgreement, AffordableHousing, onlyAffordableHousing);
        }

        return this;
    }

    public Section106 WithAdditionalAffordableHousing(bool? additionalAffordableHousing)
    {
        if (additionalAffordableHousing == null)
        {
            OperationResult.New()
                .AddValidationError(nameof(Section106Dto.AdditionalAffordableHousing), ValidationErrorMessage.MustProvideRequiredField("Additional Affordable Housing answer"))
                .CheckErrors();
        }

        if (AdditionalAffordableHousing != additionalAffordableHousing)
        {
            return new Section106(GeneralAgreement, AffordableHousing, OnlyAffordableHousing, additionalAffordableHousing);
        }

        return this;
    }

    public Section106 WithCapitalFundingEligibility(bool? capitalFundingEligibility)
    {
        if (capitalFundingEligibility == null)
        {
            OperationResult.New()
                .AddValidationError(nameof(Section106Dto.CapitalFundingEligibility), ValidationErrorMessage.MustProvideRequiredField("Capital funding Eligibility answer"))
                .CheckErrors();
        }

        if (CapitalFundingEligibility != capitalFundingEligibility)
        {
            return new Section106(GeneralAgreement, AffordableHousing, OnlyAffordableHousing, AdditionalAffordableHousing, capitalFundingEligibility);
        }

        return this;
    }

    public Section106 WithLocalAuthorityConfirmation(string? localAuthorityConfirmation)
    {
        if (localAuthorityConfirmation.IsProvided() && localAuthorityConfirmation!.Length > MaximumInputLength.LongInput)
        {
            OperationResult.New()
                .AddValidationError(nameof(LocalAuthorityConfirmation), ValidationErrorMessage.StringLengthExceeded("local authority confirmation", MaximumInputLength.LongInput))
                .CheckErrors();
        }

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
