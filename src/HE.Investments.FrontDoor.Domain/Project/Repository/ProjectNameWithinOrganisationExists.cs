using HE.Investments.Account.Shared.User;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Project.Repository;

public class ProjectNameWithinOrganisationExists : IProjectNameExists
{
    private readonly IProjectRepository _repository;

    private readonly UserAccount _userAccount;

    public ProjectNameWithinOrganisationExists(IProjectRepository repository, UserAccount userAccount)
    {
        _repository = repository;
        _userAccount = userAccount;
    }

    public async Task<bool> DoesExist(ProjectName name, CancellationToken cancellationToken)
    {
        return await _repository.DoesExist(name, _userAccount, cancellationToken);
    }
}
