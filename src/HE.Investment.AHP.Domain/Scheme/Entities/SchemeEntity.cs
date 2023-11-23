using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Scheme.Entities;

public class SchemeEntity
{
    public SchemeEntity(
        ApplicationBasicDetails application,
        SchemeFunding? funding = null,
        SectionStatus status = SectionStatus.InProgress,
        AffordabilityEvidence? affordabilityEvidence = null,
        SalesRisk? salesRisk = null,
        HousingNeeds? housingNeeds = null,
        StakeholderDiscussions? stakeholderDiscussions = null,
        StakeholderDiscussionsFiles? stakeholderDiscussionsFiles = null)
    {
        Application = application;
        Funding = funding;
        Status = status;
        AffordabilityEvidence = affordabilityEvidence;
        SalesRisk = salesRisk;
        HousingNeeds = housingNeeds;
        StakeholderDiscussions = stakeholderDiscussions;
        StakeholderDiscussionsFiles = stakeholderDiscussionsFiles ?? new StakeholderDiscussionsFiles();
    }

    public ApplicationBasicDetails Application { get; }

    public SchemeFunding? Funding { get; private set; }

    public AffordabilityEvidence? AffordabilityEvidence { get; private set; }

    public SalesRisk? SalesRisk { get; private set; }

    public HousingNeeds? HousingNeeds { get; private set; }

    public StakeholderDiscussions? StakeholderDiscussions { get; private set; }

    public StakeholderDiscussionsFiles StakeholderDiscussionsFiles { get; }

    public SectionStatus Status { get; private set; }

    public void ChangeFunding(SchemeFunding funding)
    {
        Funding = funding;
        Status = SectionStatus.InProgress;
    }

    public void ChangeAffordabilityEvidence(AffordabilityEvidence affordabilityEvidence)
    {
        AffordabilityEvidence = affordabilityEvidence;
        Status = SectionStatus.InProgress;
    }

    public void ChangeSalesRisk(SalesRisk salesRisk)
    {
        SalesRisk = salesRisk;
        Status = SectionStatus.InProgress;
    }

    public void ChangeHousingNeeds(HousingNeeds housingNeeds)
    {
        HousingNeeds = housingNeeds;
        Status = SectionStatus.InProgress;
    }

    public void ChangeStakeholderDiscussions(StakeholderDiscussions stakeholderDiscussions, IList<StakeholderDiscussionsFile> stakeholderDiscussionsFilesToAdd)
    {
        StakeholderDiscussions = stakeholderDiscussions;
        StakeholderDiscussionsFiles.AddFilesToUpload(stakeholderDiscussionsFilesToAdd);
        Status = SectionStatus.InProgress;
    }
}
