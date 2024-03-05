using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Project.CommandHandlers;

public class ProvideProjectNameCommandHandler : IRequestHandler<ProvideProjectNameCommand, OperationResult>
{
    private readonly IProjectRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    public ProvideProjectNameCommandHandler(IProjectRepository repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult> Handle(ProvideProjectNameCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var project = await _repository.GetProject(request.ProjectId, userAccount, cancellationToken);

        project.ProvideName(request.Name);

        await _repository.Save(project, userAccount, cancellationToken);
        return OperationResult.Success();
    }
}
