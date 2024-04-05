using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.FinancialDetails.Constants;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.FinancialDetails.Entities;

public class FinancialDetailsEntity : IQuestion
{
    private readonly ModificationTracker _modificationTracker = new();

    public FinancialDetailsEntity(ApplicationBasicInfo applicationBasicInfo)
    {
        SectionStatus = SectionStatus.NotStarted;
        ApplicationBasicInfo = applicationBasicInfo;
        LandStatus = new LandStatus();
        LandValue = new LandValue();
        OtherApplicationCosts = new OtherApplicationCosts();
        ExpectedContributions = new ExpectedContributionsToScheme(applicationBasicInfo.Tenure);
        PublicGrants = new PublicGrants();
        SchemeFunding = SchemeFunding.Empty();
    }

    public FinancialDetailsEntity(
        ApplicationBasicInfo applicationBasicInfo,
        SchemeFunding schemeFunding,
        LandStatus landStatus,
        LandValue landValue,
        OtherApplicationCosts otherApplicationCosts,
        ExpectedContributionsToScheme expectedContributionsToScheme,
        PublicGrants publicGrants,
        SectionStatus sectionStatus)
    {
        ApplicationBasicInfo = applicationBasicInfo;
        SchemeFunding = schemeFunding;
        LandStatus = landStatus;
        LandValue = landValue;
        OtherApplicationCosts = otherApplicationCosts;
        ExpectedContributions = expectedContributionsToScheme;
        PublicGrants = publicGrants;
        SectionStatus = sectionStatus;
    }

    public ApplicationBasicInfo ApplicationBasicInfo { get; }

    public SchemeFunding SchemeFunding { get; }

    public LandStatus LandStatus { get; private set; }

    public LandValue LandValue { get; private set; }

    public OtherApplicationCosts OtherApplicationCosts { get; private set; }

    public ExpectedContributionsToScheme ExpectedContributions { get; private set; }

    public PublicGrants PublicGrants { get; private set; }

    public SectionStatus SectionStatus { get; private set; }

    public bool IsReadOnly => ApplicationBasicInfo.IsReadOnly();

    public bool IsModified => _modificationTracker.IsModified;

    public void ProvideLandStatus(LandStatus landStatus)
    {
        LandStatus = _modificationTracker.Change(LandStatus, landStatus, MarkAsNotCompleted);
    }

    public void ProvideLandValue(LandValue landValue)
    {
        LandValue = _modificationTracker.Change(LandValue, landValue, MarkAsNotCompleted);
    }

    public void ProvideOtherApplicationCosts(OtherApplicationCosts otherApplicationCosts)
    {
        OtherApplicationCosts = _modificationTracker.Change(OtherApplicationCosts, otherApplicationCosts, MarkAsNotCompleted);
    }

    public void ProvideExpectedContributions(ExpectedContributionsToScheme expectedContribution)
    {
        ExpectedContributions = _modificationTracker.Change(ExpectedContributions, expectedContribution, MarkAsNotCompleted);
    }

    public void ProvideGrants(PublicGrants publicGrants)
    {
        PublicGrants = _modificationTracker.Change(PublicGrants, publicGrants, MarkAsNotCompleted);
    }

    public bool IsAnswered()
    {
        return LandStatus.IsAnswered() &&
               LandValue.IsAnswered() &&
               OtherApplicationCosts.IsAnswered() &&
               ExpectedContributions.IsAnswered() &&
               PublicGrants.IsAnswered() &&
               SchemeFunding.IsAnswered();
    }

    public void CompleteFinancialDetails(IsSectionCompleted isSectionCompleted)
    {
        if (isSectionCompleted == IsSectionCompleted.Undefied)
        {
            OperationResult.New().AddValidationError(nameof(IsSectionCompleted), "Select whether you have completed this section").CheckErrors();
        }

        if (isSectionCompleted == IsSectionCompleted.No)
        {
            SectionStatus = SectionStatus.InProgress;
            return;
        }

        if (!IsAnswered())
        {
            OperationResult
                .New()
                .AddValidationError(nameof(IsSectionCompleted), ValidationErrorMessage.SectionIsNotCompleted).CheckErrors();
        }

        if (ExpectedTotalCosts() != ExpectedTotalContributions())
        {
            OperationResult
                .New()
                .AddValidationError(FinancialDetailsValidationFieldNames.CostsAndFunding, FinancialDetailsValidationErrors.CostsAndFundingMismatch)
                .CheckErrors();
        }

        SectionStatus = _modificationTracker.Change(SectionStatus, SectionStatus.Completed);
    }

    public decimal? ExpectedTotalCosts()
    {
        if (OtherApplicationCosts.AreAllNotAnswered() && LandValue.CurrentLandValue.IsNotProvided())
        {
            return null;
        }

        return OtherApplicationCosts.ExpectedTotalCosts().GetValueOrDefault() + (LandValue.CurrentLandValue?.Value ?? 0);
    }

    public decimal? ExpectedTotalContributions()
    {
        if (ExpectedContributions.AreAllNotAnswered() && PublicGrants.AreAllNotAnswered() && SchemeFunding.IsNotAnswered())
        {
            return null;
        }

        return SchemeFunding.RequiredFunding.GetValueOrDefault()
               + ExpectedContributions.CalculateTotal().GetValueOrDefault()
               + PublicGrants.CalculateTotal().GetValueOrDefault();
    }

    public void MarkAsNotCompleted()
    {
        SectionStatus = _modificationTracker.Change(SectionStatus, SectionStatus.InProgress);
    }
}
