using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.Projects.Commands;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, OperationResult<ProjectId>>
{
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;
    private readonly IAccountUserContext _loanUserContext;

    public CreateProjectCommandHandler(IApplicationProjectsRepository applicationProjectsRepository, IAccountUserContext loanUserContext)
    {
        _applicationProjectsRepository = applicationProjectsRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task<OperationResult<ProjectId>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var applicationProjects =
            await _applicationProjectsRepository.GetAllAsync(request.LoanApplicationId, await _loanUserContext.GetSelectedAccount(), cancellationToken);

        var project = applicationProjects.AddEmptyProject();

        await _applicationProjectsRepository.SaveAsync(request.LoanApplicationId, project, cancellationToken);

        return OperationResult.Success(project.Id);
    }
}
