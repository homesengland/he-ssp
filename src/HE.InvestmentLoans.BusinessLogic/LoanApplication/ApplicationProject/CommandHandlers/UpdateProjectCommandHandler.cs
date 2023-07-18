using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.CommandHandlers;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand>
{
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;
    private readonly ILoanUserContext _loanUserContext;

    public UpdateProjectCommandHandler(IApplicationProjectsRepository applicationProjectsRepository, ILoanUserContext loanUserContext)
    {
        _applicationProjectsRepository = applicationProjectsRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        _applicationProjectsRepository.UpdateProject(
            request.LoanApplicationEntity,
            request.Project,
            new UserAccount(_loanUserContext.UserGlobalId, await _loanUserContext.GetSelectedAccountId().ConfigureAwait(false)));
    }
}
