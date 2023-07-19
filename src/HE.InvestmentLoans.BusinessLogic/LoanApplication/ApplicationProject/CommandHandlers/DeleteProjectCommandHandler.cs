using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.CommandHandlers;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;
    private readonly ILoanUserContext _loanUserContext;

    public DeleteProjectCommandHandler(IApplicationProjectsRepository applicationProjectsRepository, ILoanUserContext loanUserContext)
    {
        _applicationProjectsRepository = applicationProjectsRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var applicationProjects = _applicationProjectsRepository.GetAll(
                                                                    request.LoanApplicationId,
                                                                    new UserAccount(_loanUserContext.UserGlobalId, await _loanUserContext.GetSelectedAccountId()));

        _applicationProjectsRepository.Delete(applicationProjects, request.ProjectId);

        _applicationProjectsRepository.Save(applicationProjects);
    }
}
