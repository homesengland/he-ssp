using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Domain.Entities;

namespace HE.Investments.AHP.Consortium.Domain.Repositories;

public interface IConsortiumRepository : IIsPartOfConsortium
{
    Task<ConsortiumEntity> GetConsortium(ConsortiumId consortiumId);

    Task<ConsortiumEntity> Save(ConsortiumEntity consortiumEntity);
}
