using HE.Investment.AHP.Domain.Scheme.ValueObjects;

namespace HE.Investment.AHP.Domain.Scheme.Entities;

public class SchemeEntity
{
    public SchemeEntity(SchemeFunding funding)
    {
        Funding = funding;
    }

    public SchemeFunding Funding { get; private set; }

    public AffordabilityEvidence? AffordabilityEvidence { get; private set; }

    public SalesRisk? SalesRisk { get; private set; }

    public HousingNeeds? HousingNeeds { get; private set; }

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
}
