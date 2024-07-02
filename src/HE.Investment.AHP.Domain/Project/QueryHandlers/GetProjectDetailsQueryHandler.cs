using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Contract.Project.Queries;
using HE.Investment.AHP.Domain.Project.Repositories;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Extensions;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Contract.Config;
using HE.Investments.Programme.Contract.Queries;
using MediatR;

namespace HE.Investment.AHP.Domain.Project.QueryHandlers;

public class GetProjectDetailsQueryHandler : IRequestHandler<GetProjectDetailsQuery, ProjectDetailsModel>
{
    private readonly IProjectRepository _projectRepository;

    private readonly IMediator _mediator;

    private readonly IConsortiumUserContext _userContext;

    private readonly IConsortiumAccessContext _accessContext;

    private readonly IProgrammeSettings _programmeSettings;

    public GetProjectDetailsQueryHandler(
        IProjectRepository projectRepository,
        IMediator mediator,
        IConsortiumUserContext userContext,
        IProgrammeSettings programmeSettings,
        IConsortiumAccessContext accessContext)
    {
        _projectRepository = projectRepository;
        _mediator = mediator;
        _userContext = userContext;
        _programmeSettings = programmeSettings;
        _accessContext = accessContext;
    }

    public async Task<ProjectDetailsModel> Handle(GetProjectDetailsQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _userContext.GetSelectedAccount();
        var project = await _projectRepository.GetProjectApplications(request.ProjectId, userAccount, cancellationToken);
        var programme = await _mediator.Send(new GetProgrammeQuery(ProgrammeId.From(_programmeSettings.AhpProgrammeId)), cancellationToken);

        var applications = project.Applications
            .OrderByDescending(x => x.LastModificationOn)
            .Select(x => new ApplicationProjectModel(
                x.Id,
                x.Name.ToString(),
                x.ApplicationStatus,
                x.Funding.RequiredFunding,
                x.Funding.HousesToDeliver,
                x.LocalAuthorityName))
            .ToList();

        return new ProjectDetailsModel(
            project.Id,
            project.Name.Value,
            programme.Name,
            userAccount.Organisation!.RegisteredCompanyName,
            new PaginationResult<ApplicationProjectModel>(
                applications.TakePage(request.ApplicationPaginationRequest).ToList(),
                request.ApplicationPaginationRequest.Page,
                request.ApplicationPaginationRequest.ItemsPerPage,
                applications.Count),
            !await _accessContext.CanEditApplication());
    }
}
