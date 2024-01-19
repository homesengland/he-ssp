using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Common.Contract;
using ApplicationSection = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationSection;

namespace HE.Investment.AHP.Domain.Tests.Application.Entities.ApplicationEntityTests;

public class ApplicationEntityBuilder
{
    private readonly AhpApplicationId _id = new("1");

    private readonly ApplicationName _name = new("zapytanko");

    private readonly ApplicationReferenceNumber _reference = new("REF123");

    private ApplicationStatus _status = ApplicationStatus.New;

    private IList<ApplicationSection>? _sections;

    public static ApplicationEntityBuilder New() => new();

    public ApplicationEntityBuilder WithSections(params ApplicationSection[] sections)
    {
        _sections = sections.ToList();
        return this;
    }

    public ApplicationEntityBuilder WithApplicationStatus(ApplicationStatus status)
    {
        _status = status;
        return this;
    }

    public ApplicationEntity Build()
    {
        return new ApplicationEntity(_id, _name, _status, _reference, null, null, new ApplicationSections(_sections ?? new List<ApplicationSection>()));
    }
}
