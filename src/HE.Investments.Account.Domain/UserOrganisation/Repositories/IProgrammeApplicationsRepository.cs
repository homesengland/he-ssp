using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.Shared.User;

namespace HE.Investments.Account.Domain.UserOrganisation.Repositories;

public interface IProgrammeApplicationsRepository
{
    Task<IList<Programme>> GetAllProgrammes(UserAccount userAccount, CancellationToken cancellationToken);

    Task<IList<Programme>> GetLoanProgrammes(UserAccount userAccount, CancellationToken cancellationToken);

    Task<bool> HasAnyAhpApplication(UserAccount userAccount, CancellationToken cancellationToken);
}
