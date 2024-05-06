using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Domain.Entities;

namespace HE.Investments.AHP.Consortium.Domain.Repositories;

public interface IDraftConsortiumRepository
{
    DraftConsortiumEntity? Get(ConsortiumId consortiumId, bool throwException = false);

    void Create(ConsortiumEntity consortium);

    void Save(DraftConsortiumEntity consortium);

    void Delete(DraftConsortiumEntity consortium);
}
