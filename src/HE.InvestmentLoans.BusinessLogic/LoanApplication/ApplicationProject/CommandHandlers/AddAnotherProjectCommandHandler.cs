using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.CommandHandlers;

public class AddAnotherProjectCommandHandler : IRequestHandler<AddAnotherProjectCommand>
{
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;
    private readonly ILoanUserContext _loanUserContext;

    public AddAnotherProjectCommandHandler(IApplicationProjectsRepository applicationProjectsRepository, ILoanUserContext loanUserContext)
    {
        _applicationProjectsRepository = applicationProjectsRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task Handle(AddAnotherProjectCommand request, CancellationToken cancellationToken)
    {
        _applicationProjectsRepository.AddAnotherProject(
            request.LoanApplicationEntity,
            new UserAccount(_loanUserContext.UserGlobalId, await _loanUserContext.GetSelectedAccountId().ConfigureAwait(false)));
    }
}
