using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Project.CommandHandlers;

public class ProvideProjectNameCommandHandler : ProjectBaseCommandHandler<ProvideProjectNameCommand>
{
    public ProvideProjectNameCommandHandler(IProjectRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override async Task PerformAsync(ProjectEntity project, ProvideProjectNameCommand request, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var projectName = new ProjectName(request.Name ?? string.Empty);
        var projectNameExists = new ProjectNameWithinOrganisationExists(ProjectRepository, userAccount);

        await project.ProvideName(projectName, projectNameExists, cancellationToken);
    }
}
