using HE.Investments.Account.Shared.User;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investments.Account.Domain.UserOrganisation.Repositories;

public interface IProgrammeApplicationsRepository
{
    Task<IList<Contract.UserOrganisation.Programme>> GetAllProgrammes(ConsortiumUserAccount userAccount, CancellationToken cancellationToken);

    Task<IList<Contract.UserOrganisation.Programme>> GetLoanProgrammes(UserAccount userAccount, CancellationToken cancellationToken);

    Task<bool> HasAnyAhpApplication(ConsortiumUserAccount userAccount, CancellationToken cancellationToken);
}
