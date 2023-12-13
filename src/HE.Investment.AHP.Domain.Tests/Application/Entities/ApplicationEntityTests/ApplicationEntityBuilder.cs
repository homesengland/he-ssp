using System.Collections.Generic;
using System.Linq;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Common.Contract;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Tests.Application.Entities.ApplicationEntityTests;

public class ApplicationEntityBuilder
{
    private readonly ApplicationId _id = new("1");

    private readonly ApplicationName _name = new("zapytanko");

    private readonly ApplicationStatus _status = ApplicationStatus.New;

    private readonly ApplicationReferenceNumber _reference = new("REF123");

    private IList<ApplicationSection>? _sections;

    public static ApplicationEntityBuilder New() => new();

    public ApplicationEntityBuilder WithSections(params ApplicationSection[] sections)
    {
        _sections = sections.ToList();
        return this;
    }

    public ApplicationEntity Build()
    {
        return new ApplicationEntity(_id, _name, _status, _reference, null, null, new ApplicationSections(_sections ?? new List<ApplicationSection>()));
    }
}
