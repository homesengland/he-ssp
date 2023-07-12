using HE.InvestmentLoans.BusinessLogic.Application.Project.Repositories;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Application.Project.CommandHandlers;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, LoanApplicationViewModel>
{
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;

    public DeleteProjectCommandHandler(IApplicationProjectsRepository applicationProjectsRepository)
    {
        _applicationProjectsRepository = applicationProjectsRepository;
    }

    public Task<LoanApplicationViewModel> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var result = _applicationProjectsRepository.DeleteProject(request.LoanApplicationId, request.ProjectId);

        return Task.FromResult(result);
    }
}
