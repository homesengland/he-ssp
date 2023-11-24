using HE.Investments.Account.Shared;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.Contract.Projects.Queries;
using HE.Investments.Loans.Contract.Projects.ViewModels;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.Projects.QueryHandlers;
public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, ProjectViewModel>
{
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;
    private readonly IAccountUserContext _loanUserContext;

    public GetProjectQueryHandler(IApplicationProjectsRepository applicationProjectsRepository, IAccountUserContext loanUserContext)
    {
        _applicationProjectsRepository = applicationProjectsRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task<ProjectViewModel> Handle(GetProjectQuery request, CancellationToken cancellationToken)
    {
        var project = await _applicationProjectsRepository.GetByIdAsync(request.ProjectId, await _loanUserContext.GetSelectedAccount(), request.ProjectFieldsSet, cancellationToken);

        return ProjectMapper.MapToViewModel(project, request.ApplicationId);
    }
}
