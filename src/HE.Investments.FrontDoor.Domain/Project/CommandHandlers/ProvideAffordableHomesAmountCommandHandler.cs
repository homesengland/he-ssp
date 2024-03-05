using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Project.CommandHandlers;

public class ProvideAffordableHomesAmountCommandHandler : IRequestHandler<ProvideAffordableHomesAmountCommand, OperationResult>
{
    private readonly IProjectRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    public ProvideAffordableHomesAmountCommandHandler(IProjectRepository repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult> Handle(ProvideAffordableHomesAmountCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var project = await _repository.GetProject(request.ProjectId, userAccount, cancellationToken);

        project.ProvideAffordableHomesAmount(new ProjectAffordableHomesAmount(request.AffordableHomesAmount));

        await _repository.Save(project, userAccount, cancellationToken);
        return OperationResult.Success();
    }
}
