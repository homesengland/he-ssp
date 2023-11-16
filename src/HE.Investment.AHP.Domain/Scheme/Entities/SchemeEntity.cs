using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Scheme.Entities;

public class SchemeEntity
{
    public SchemeEntity(
        ApplicationBasicDetails application,
        SchemeFunding funding,
        SectionStatus status = SectionStatus.InProgress,
        AffordabilityEvidence? affordabilityEvidence = null,
        SalesRisk? salesRisk = null,
        HousingNeeds? housingNeeds = null,
        StakeholderDiscussions? stakeholderDiscussions = null)
    {
        Application = application;
        Funding = funding;
        Status = status;
        AffordabilityEvidence = affordabilityEvidence;
        SalesRisk = salesRisk;
        HousingNeeds = housingNeeds;
        StakeholderDiscussions = stakeholderDiscussions;
    }

    public ApplicationBasicDetails Application { get; }

    public SchemeFunding Funding { get; private set; }

    public AffordabilityEvidence? AffordabilityEvidence { get; private set; }

    public SalesRisk? SalesRisk { get; private set; }

    public HousingNeeds? HousingNeeds { get; private set; }

    public StakeholderDiscussions? StakeholderDiscussions { get; private set; }

    public SectionStatus Status { get; private set; }

    public void ChangeFunding(SchemeFunding funding)
    {
        Funding = funding;
    }

    public void ChangeAffordabilityEvidence(AffordabilityEvidence affordabilityEvidence)
    {
        AffordabilityEvidence = affordabilityEvidence;
    }

    public void ChangeSalesRisk(SalesRisk salesRisk)
    {
        SalesRisk = salesRisk;
    }

    public void ChangeHousingNeeds(HousingNeeds housingNeeds)
    {
        HousingNeeds = housingNeeds;
    }

    public void ChangeStakeholderDiscussions(StakeholderDiscussions stakeholderDiscussions)
    {
        StakeholderDiscussions = stakeholderDiscussions;
    }
}
