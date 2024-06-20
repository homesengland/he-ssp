using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.Domain.UserOrganisation.Storage;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;

namespace HE.Investments.Account.Domain.UserOrganisation.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly IProjectContext _projectContext;

    public ProjectRepository(IProjectContext projectContext)
    {
        _projectContext = projectContext;
    }

    public async Task<IList<UserProject>> GetUserProjects(UserAccount userAccount, CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId();
        var projects = userAccount.CanViewAllApplications()
            ? await _projectContext.GetOrganisationProjects(organisationId.ToString(), cancellationToken)
            : await _projectContext.GetUserProjects(userAccount.UserGlobalId.ToString(), organisationId.ToString(), cancellationToken);

        return projects
            .OrderByDescending(x => x.CreatedOn ?? DateTime.MinValue)
            .Select(x => new UserProject(
                HeProjectId.From(x.ProjectId),
                x.ProjectName,
                ApplicationStatus.Draft.GetDescription()))
            .ToList();
    }
}
