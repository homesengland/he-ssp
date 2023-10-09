using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Utils.Enums;
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
        var applicationProjects =
            await _applicationProjectsRepository.GetById(request.Id, await _loanUserContext.GetSelectedAccount(), ProjectFieldsSet.GetEmpty, cancellationToken);

        var projectId = applicationProjects.AddEmptyProject();

        await _applicationProjectsRepository.SaveAsync(applicationProjects, projectId, await _loanUserContext.GetSelectedAccount(), cancellationToken);

        return OperationResult.Success(projectId);
    }
}
