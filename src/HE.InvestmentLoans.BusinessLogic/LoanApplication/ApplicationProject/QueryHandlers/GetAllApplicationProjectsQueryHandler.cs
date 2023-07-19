using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.QueryHandlers;

public class GetAllApplicationProjectsQueryHandler : IRequestHandler<GetAllApplicationProjectsQuery, ApplicationProjects>
{
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;
    private readonly ILoanUserContext _loanUserContext;

    public GetAllApplicationProjectsQueryHandler(IApplicationProjectsRepository applicationProjectsRepository, ILoanUserContext loanUserContext)
    {
        _applicationProjectsRepository = applicationProjectsRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task<ApplicationProjects> Handle(GetAllApplicationProjectsQuery request, CancellationToken cancellationToken)
    {
        return _applicationProjectsRepository.GetAll(
                                                request.LoanApplicationId,
                                                new UserAccount(_loanUserContext.UserGlobalId, await _loanUserContext.GetSelectedAccountId()));
    }
}
