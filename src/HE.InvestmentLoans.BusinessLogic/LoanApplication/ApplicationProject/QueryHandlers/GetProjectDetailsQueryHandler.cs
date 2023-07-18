using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.QueryHandlers;

public class GetProjectDetailsQueryHandler : IRequestHandler<GetProjectDetailsQuery, Project>
{
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;
    private readonly ILoanUserContext _loanUserContext;

    public GetProjectDetailsQueryHandler(IApplicationProjectsRepository applicationProjectsRepository, ILoanUserContext loanUserContext)
    {
        _applicationProjectsRepository = applicationProjectsRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task<Project> Handle(GetProjectDetailsQuery request, CancellationToken cancellationToken)
    {
        return _applicationProjectsRepository.GetProjectDetails(
                                                request.LoanApplicationId,
                                                request.ProjectId,
                                                new UserAccount(_loanUserContext.UserGlobalId, await _loanUserContext.GetSelectedAccountId().ConfigureAwait(false)));
    }
}
