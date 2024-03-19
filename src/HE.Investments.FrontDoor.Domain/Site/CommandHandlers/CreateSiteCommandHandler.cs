using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Contract.Site;
using HE.Investments.FrontDoor.Contract.Site.Commands;
using HE.Investments.FrontDoor.Domain.Site.Repository;
using HE.Investments.FrontDoor.Domain.Site.ValueObjects;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Site.CommandHandlers;

public class CreateSiteCommandHandler : IRequestHandler<CreateSiteCommand, OperationResult<FrontDoorSiteId>>
{
    private readonly ISiteRepository _siteRepository;

    private readonly IAccountUserContext _accountUserContext;

    public CreateSiteCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
    {
        _siteRepository = siteRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult<FrontDoorSiteId>> Handle(CreateSiteCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var projectSites = await _siteRepository.GetProjectSites(request.ProjectId, userAccount, cancellationToken);

        var newProject = projectSites.CreateNewSite(new SiteName(request.Name ?? string.Empty));
        await _siteRepository.Save(newProject, userAccount, cancellationToken);

        return new OperationResult<FrontDoorSiteId>(newProject.Id);
    }
}
