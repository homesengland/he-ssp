using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.Shared.User;

namespace HE.Investments.Account.Domain.UserOrganisation.Repositories;

public interface IProgrammeRepository
{
    Task<IList<Programme>> GetAllProgrammes(UserAccount userAccount, CancellationToken cancellationToken);
}
