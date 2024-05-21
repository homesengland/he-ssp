using HE.Investments.Account.Shared.User;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.Consortium.Domain.Repositories;

public interface IConsortiumRepository : IIsPartOfConsortium
{
    Task<ConsortiumEntity> GetConsortium(ConsortiumId consortiumId, UserAccount userAccount, CancellationToken cancellationToken);

    Task<IList<ConsortiumEntity>> GetConsortiumsListByMemberId(OrganisationId organisationId, CancellationToken cancellationToken);

    Task<ConsortiumEntity> Save(ConsortiumEntity consortiumEntity, UserAccount userAccount, CancellationToken cancellationToken);
}
