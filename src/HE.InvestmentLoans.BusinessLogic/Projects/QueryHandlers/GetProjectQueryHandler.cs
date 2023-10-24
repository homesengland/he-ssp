using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Projects.Queries;
using HE.InvestmentLoans.Contract.Projects.ViewModels;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Projects.QueryHandlers;
public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, ProjectViewModel>
{
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;
    private readonly ILoanUserContext _loanUserContext;

    public GetProjectQueryHandler(IApplicationProjectsRepository applicationProjectsRepository, ILoanUserContext loanUserContext)
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
