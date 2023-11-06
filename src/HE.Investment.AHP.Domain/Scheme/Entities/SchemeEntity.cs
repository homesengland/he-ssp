using HE.Investment.AHP.Domain.Scheme.ValueObjects;

namespace HE.Investment.AHP.Domain.Scheme.Entities;

public class SchemeEntity
{
    public SchemeEntity(SchemeId id, SchemeFunding funding)
    {
        Id = id;
        Funding = funding;
    }

    public SchemeId Id { get; }

    public SchemeFunding Funding { get; private set; }

    public AffordabilityEvidence? AffordabilityEvidence { get; private set; }

    public SalesRisk? SalesRisk { get; private set; }

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
}
