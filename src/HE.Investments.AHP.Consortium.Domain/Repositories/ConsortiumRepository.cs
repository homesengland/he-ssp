using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Domain.Entities;

namespace HE.Investments.AHP.Consortium.Domain.Repositories;

public class ConsortiumRepository : IConsortiumRepository
{
    public Task<ConsortiumEntity> GetConsortium(ConsortiumId consortiumId)
    {
        throw new NotImplementedException();
    }

    public Task<ConsortiumEntity> Save(ConsortiumEntity consortiumEntity)
    {
        if (consortiumEntity.Id.IsNew)
        {
            consortiumEntity.SetId(new ConsortiumId(Guid.NewGuid().ToString()));
        }

        return Task.FromResult(consortiumEntity);
    }

    public Task<bool> IsPartOfConsortiumForProgramme(ProgrammeId programmeId, OrganisationId organisationId)
    {
        return Task.FromResult(false);
    }
}
