using HE.Investment.AHP.Domain.Application.ValueObjects;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Application.Entities;

public class ApplicationEntity
{
    public ApplicationEntity(ApplicationId id, ApplicationName name, ApplicationTenure? tenure = null)
    {
        Id = id;
        Name = name;
        Tenure = tenure;
    }

    public ApplicationId Id { get; }

    public ApplicationName Name { get; private set; }

    public ApplicationTenure? Tenure { get; private set; }

    public void ChangeName(ApplicationName name)
    {
        Name = name;
    }

    public void ChangeTenure(ApplicationTenure tenure)
    {
        Tenure = tenure;
    }
}
