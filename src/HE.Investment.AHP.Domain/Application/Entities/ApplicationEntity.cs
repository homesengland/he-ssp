using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Contract;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Application.Entities;

public class ApplicationEntity
{
    public ApplicationEntity(
        ApplicationId id,
        ApplicationName name,
        ApplicationTenure? tenure,
        ApplicationSections sections)
    {
        Id = id;
        Name = name;
        Tenure = tenure;
        Sections = sections;
    }

    public ApplicationId Id { get; private set; }

    public ApplicationName Name { get; private set; }

    public ApplicationTenure? Tenure { get; private set; }

    public ApplicationSections Sections { get; private set; }

    public static ApplicationEntity New(ApplicationName name) => new(ApplicationId.Empty(), name, null, new ApplicationSections());

    public void SetId(ApplicationId newId)
    {
        if (!Id.IsEmpty())
        {
            throw new DomainException("Id cannot be modified", CommonErrorCodes.IdCannotBeModified);
        }

        Id = newId;
    }

    public void ChangeName(ApplicationName name)
    {
        Name = name;
    }

    public void ChangeTenure(ApplicationTenure tenure)
    {
        Tenure = tenure;
    }
}
