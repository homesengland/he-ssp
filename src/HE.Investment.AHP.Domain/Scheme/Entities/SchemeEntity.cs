using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Scheme.Entities;

public class SchemeEntity
{
    private readonly ModificationTracker _modificationTracker = new();

    public SchemeEntity(
        ApplicationBasicDetails application,
        SchemeFunding funding,
        SectionStatus status,
        AffordabilityEvidence affordabilityEvidence,
        SalesRisk salesRisk,
        HousingNeeds housingNeeds,
        StakeholderDiscussions stakeholderDiscussions)
    {
        Status = status;
        Application = application;
        Funding = funding;
        AffordabilityEvidence = affordabilityEvidence;
        SalesRisk = salesRisk;
        HousingNeeds = housingNeeds;
        StakeholderDiscussions = stakeholderDiscussions;
    }

    public ApplicationBasicDetails Application { get; }

    public SchemeFunding Funding { get; private set; }

    public AffordabilityEvidence AffordabilityEvidence { get; private set; }

    public SalesRisk SalesRisk { get; private set; }

    public HousingNeeds HousingNeeds { get; private set; }

    public StakeholderDiscussions StakeholderDiscussions { get; }

    public SectionStatus Status { get; private set; }

    public bool IsModified => _modificationTracker.IsModified || StakeholderDiscussions.IsModified;

    public void ChangeFunding(SchemeFunding funding)
    {
        Funding = _modificationTracker.Change(Funding, funding, SetInProgress);
    }

    public void ChangeAffordabilityEvidence(AffordabilityEvidence affordabilityEvidence)
    {
        AffordabilityEvidence = _modificationTracker.Change(AffordabilityEvidence, affordabilityEvidence, SetInProgress);
    }

    public void ChangeSalesRisk(SalesRisk salesRisk)
    {
        SalesRisk = _modificationTracker.Change(SalesRisk, salesRisk, SetInProgress);
    }

    public void ChangeHousingNeeds(HousingNeeds housingNeeds)
    {
        HousingNeeds = _modificationTracker.Change(HousingNeeds, housingNeeds, SetInProgress);
    }

    public void ChangeStakeholderDiscussions(StakeholderDiscussionsDetails stakeholderDiscussionsDetails, LocalAuthoritySupportFile? localAuthoritySupportFile)
    {
        StakeholderDiscussions.ChangeStakeholderDiscussionsDetails(stakeholderDiscussionsDetails);
        if (localAuthoritySupportFile != null)
        {
            StakeholderDiscussions.ChangeLocalAuthoritySupportFile(localAuthoritySupportFile);
        }

        if (StakeholderDiscussions.IsModified)
        {
            SetInProgress();
        }
    }

    public void RemoveStakeholderDiscussionsFile(FileId fileId)
    {
        StakeholderDiscussions.MarkFileToRemove(fileId);
        SetInProgress();
    }

    public void Complete()
    {
        var operationResult = new OperationResult();
        operationResult.Aggregate(() => Funding.CheckIsComplete());
        operationResult.Aggregate(() => AffordabilityEvidence.CheckIsComplete());
        operationResult.Aggregate(() => SalesRisk.CheckIsComplete());
        operationResult.Aggregate(() => HousingNeeds.CheckIsComplete());
        operationResult.Aggregate(() => StakeholderDiscussions.CheckIsComplete());

        operationResult.CheckErrors();

        Status = _modificationTracker.Change(Status, SectionStatus.Completed);
    }

    public void UnComplete()
    {
        Status = _modificationTracker.Change(Status, SectionStatus.InProgress);
    }

    private void SetInProgress()
    {
        Status = _modificationTracker.Change(Status, SectionStatus.InProgress);
    }
}
