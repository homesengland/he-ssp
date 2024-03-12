using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.Shared.User;

namespace HE.Investments.Account.Domain.UserOrganisation.Repositories;

public interface IProjectRepository
{
    Task<IList<UserProject>> GetUserProjects(UserAccount userAccount, CancellationToken cancellationToken);
}
