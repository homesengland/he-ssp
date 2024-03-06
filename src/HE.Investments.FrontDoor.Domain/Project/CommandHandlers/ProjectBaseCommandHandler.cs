using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Project.CommandHandlers;

public abstract class ProjectBaseCommandHandler<TRequest> : IRequestHandler<TRequest, OperationResult>
    where TRequest : IProvideProjectDetailsCommand
{
    protected ProjectBaseCommandHandler(IProjectRepository projectRepository, IAccountUserContext accountUserContext)
    {
        ProjectRepository = projectRepository;
        AccountUserContext = accountUserContext;
    }

    protected IProjectRepository ProjectRepository { get; }

    protected IAccountUserContext AccountUserContext { get; }

    public async Task<OperationResult> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var userAccount = await AccountUserContext.GetSelectedAccount();
        var project = await ProjectRepository.GetProject(request.ProjectId, userAccount, cancellationToken);

        Perform(project, request);
        await PerformAsync(project, request, cancellationToken);

        await ProjectRepository.Save(project, userAccount, cancellationToken);
        return OperationResult.Success();
    }

    protected virtual void Perform(ProjectEntity project, TRequest request)
    {
    }

    protected virtual Task PerformAsync(ProjectEntity project, TRequest request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
