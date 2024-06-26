using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Project.Queries;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Site.Repository;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Project.QueryHandlers;

public class GetProjectDetailsQueryHandler : IRequestHandler<GetProjectDetailsQuery, ProjectDetails>
{
    private readonly IProjectRepository _projectRepository;

    private readonly ISiteRepository _siteRepository;

    private readonly IAccountUserContext _accountUserContext;

    public GetProjectDetailsQueryHandler(IProjectRepository projectRepository, IAccountUserContext accountUserContext, ISiteRepository siteRepository)
    {
        _projectRepository = projectRepository;
        _accountUserContext = accountUserContext;
        _siteRepository = siteRepository;
    }

    public async Task<ProjectDetails> Handle(GetProjectDetailsQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var project = await _projectRepository.GetProject(request.ProjectId, userAccount, cancellationToken);
        var projectSite = await _siteRepository.GetProjectSites(request.ProjectId, userAccount, cancellationToken);

        return new ProjectDetails
        {
            Id = project.Id,
            Name = project.Name.Value,
            IsEnglandHousingDelivery = project.IsEnglandHousingDelivery,
            OrganisationHomesBuilt = project.OrganisationHomesBuilt?.ToString(),
            IsSiteIdentified = project.IsSiteIdentified?.Value,
            GeographicFocus = project.GeographicFocus.GeographicFocus,
            IsSupportRequired = project.IsSupportRequired?.Value,
            IsFundingRequired = project.IsFundingRequired?.Value,
            SupportActivityTypes = project.SupportActivities.Values,
            AffordableHomesAmount = project.AffordableHomesAmount.AffordableHomesAmount,
            InfrastructureTypes = project.Infrastructure.Values,
            Regions = project.Regions.Values,
            HomesNumber = project.HomesNumber?.ToString(),
            RequiredFunding = project.RequiredFunding.Value,
            IsProfit = project.IsProfit.Value,
            LastSiteId = projectSite.LastSiteId(),
            ExpectedStartDate = DateDetails.FromDateTime(project.ExpectedStartDate.Value?.ToDateTime(TimeOnly.MinValue)),
            LocalAuthorityCode = project.LocalAuthority?.Code.Value,
            LocalAuthorityName = project.LocalAuthority?.Name,
        };
    }
}
