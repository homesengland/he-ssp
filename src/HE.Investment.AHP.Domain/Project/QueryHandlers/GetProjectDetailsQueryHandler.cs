using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Contract.Project.Queries;
using HE.Investment.AHP.Domain.Programme;
using HE.Investment.AHP.Domain.Project.Repositories;
using HE.Investment.AHP.Domain.Project.ValueObjects;
using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Extensions;
using MediatR;

namespace HE.Investment.AHP.Domain.Project.QueryHandlers;

public class GetProjectDetailsQueryHandler : IRequestHandler<GetProjectDetailsQuery, ProjectDetailsModel>
{
    private readonly IProjectRepository _projectRepository;

    private readonly IAhpUserContext _userContext;

    private readonly IAhpProgrammeRepository _programmeRepository;

    public GetProjectDetailsQueryHandler(
        IProjectRepository projectRepository,
        IAhpUserContext userContext,
        IAhpProgrammeRepository programmeRepository)
    {
        _projectRepository = projectRepository;
        _userContext = userContext;
        _programmeRepository = programmeRepository;
    }

    public async Task<ProjectDetailsModel> Handle(GetProjectDetailsQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _userContext.GetSelectedAccount();
        var project = await _projectRepository.GetProject(request.ProjectId, userAccount, cancellationToken);

        var applications = project.Applications
            .OrderByDescending(x => x.LastModificationOn)
            .Select(x => new ApplicationProjectModel(
                x.Id,
                x.Name.ToString(),
                x.ApplicationStatus,
                x.Funding.RequiredFunding,
                x.Funding.HousesToDeliver))
            .ToList();

        return new ProjectDetailsModel(
            project.Id,
            project.Name.Value,
            (await _programmeRepository.GetProgramme(cancellationToken)).Name,
            userAccount.Organisation!.RegisteredCompanyName,
            new PaginationResult<ApplicationProjectModel>(
                applications.TakePage(request.ApplicationPaginationRequest).ToList(),
                request.ApplicationPaginationRequest.Page,
                request.ApplicationPaginationRequest.ItemsPerPage,
                applications.Count),
            !userAccount.CanEditApplication);
    }
}
