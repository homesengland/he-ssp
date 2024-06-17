using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Contract.Project.Queries;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Services;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Project.CommandHandlers;

public class DetermineProjectDecisionCommandHandler : IRequestHandler<DetermineProjectDecisionCommand, (OperationResult Result, ApplicationType Type)>
{
    private readonly IProjectRepository _projectRepository;

    private readonly IAccountUserContext _accountUserContext;

    private readonly IEligibilityService _eligibilityService;

    public DetermineProjectDecisionCommandHandler(
        IProjectRepository projectRepository,
        IAccountUserContext accountUserContext,
        IEligibilityService eligibilityService)
    {
        _projectRepository = projectRepository;
        _accountUserContext = accountUserContext;
        _eligibilityService = eligibilityService;
    }

    public async Task<(OperationResult Result, ApplicationType Type)> Handle(DetermineProjectDecisionCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var project = await _projectRepository.GetProject(request.ProjectId, userAccount, cancellationToken);
        var result = await project.Complete(_eligibilityService);
        await _projectRepository.Save(project, userAccount, cancellationToken);
        return result;
    }
}
