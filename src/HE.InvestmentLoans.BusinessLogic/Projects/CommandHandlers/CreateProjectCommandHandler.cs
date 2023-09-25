using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Projects.Commands;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, OperationResult<ProjectId>>
{
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;
    private readonly ILoanUserContext _loanUserContext;

    public CreateProjectCommandHandler(IApplicationProjectsRepository applicationProjectsRepository, ILoanUserContext loanUserContext)
    {
        _applicationProjectsRepository = applicationProjectsRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task<OperationResult<ProjectId>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var applicationProjects = await _applicationProjectsRepository.GetById(request.Id, await _loanUserContext.GetSelectedAccount(), cancellationToken)
            ?? throw new NotFoundException(nameof(ApplicationProjects), request.Id);

        var projectId = applicationProjects.AddProject();

        await _applicationProjectsRepository.SaveAsync(applicationProjects, await _loanUserContext.GetSelectedAccount(), cancellationToken);

        return OperationResult.Success(projectId);
    }
}
