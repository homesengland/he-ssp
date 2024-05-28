using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.Factories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Common;

public class ApplicationBasicInfo
{
    private readonly ApplicationState _applicationState;

    public ApplicationBasicInfo(
        AhpApplicationId id,
        SiteId siteId,
        ApplicationName name,
        Tenure tenure,
        ApplicationStatus status,
        ApplicationSections sections,
        IApplicationStateFactory applicationStateFactory)
    {
        Id = id;
        SiteId = siteId;
        Name = name;
        Tenure = tenure;
        Status = status;
        Sections = sections;
        _applicationState = applicationStateFactory.Create(Status);
    }

    public AhpApplicationId Id { get; }

    public SiteId SiteId { get; }

    public ApplicationName Name { get; }

    public Tenure Tenure { get; }

    public ApplicationStatus Status { get; }

    public ApplicationSections Sections { get; }

    public IEnumerable<AhpApplicationOperation> AllowedOperations => _applicationState.AllowedOperations;
}
