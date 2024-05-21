using HE.Investments.Account.Shared.User;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Domain.Entities;

namespace HE.Investments.AHP.Consortium.Domain.Repositories;

public interface IDraftConsortiumRepository
{
    DraftConsortiumEntity? Get(ConsortiumId consortiumId, UserAccount userAccount, bool throwException = false);

    void Create(ConsortiumEntity consortium, UserAccount userAccount);

    void Save(DraftConsortiumEntity consortium, UserAccount userAccount);

    void Delete(DraftConsortiumEntity consortium, UserAccount userAccount);
}
