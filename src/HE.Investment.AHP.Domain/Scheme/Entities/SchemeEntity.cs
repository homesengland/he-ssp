using HE.Investment.AHP.Domain.Scheme.ValueObjects;

namespace HE.Investment.AHP.Domain.Scheme.Entities;

public class SchemeEntity
{
    public SchemeEntity(SchemeId id, SchemeName name, SchemeTenure? tenure = null)
    {
        Id = id;
        Name = name;
        Tenure = tenure;
    }

    public SchemeId Id { get; }

    public SchemeName Name { get; private set; }

    public SchemeTenure? Tenure { get; private set; }

    public void ChangeName(SchemeName name)
    {
        Name = name;
    }

    public void ChangeTenure(SchemeTenure tenure)
    {
        Tenure = tenure;
    }
}
