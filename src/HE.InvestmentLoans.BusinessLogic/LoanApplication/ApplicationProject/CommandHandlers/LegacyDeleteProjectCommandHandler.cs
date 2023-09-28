using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.CommandHandlers;

public class LegacyDeleteProjectCommandHandler : IRequestHandler<LegacyDeleteProjectCommand, LoanApplicationViewModel>
{
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;

    public LegacyDeleteProjectCommandHandler(IApplicationProjectsRepository applicationProjectsRepository)
    {
        _applicationProjectsRepository = applicationProjectsRepository;
    }

    public Task<LoanApplicationViewModel> Handle(LegacyDeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var result = _applicationProjectsRepository.LegacyDeleteProject(request.LoanApplicationId, request.ProjectId);

        return Task.FromResult(result);
    }
}
