using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investments.AHP.Consortium.Domain.Repositories;

public class ConsortiumRepository : IConsortiumRepository
{
    private static readonly IDictionary<ConsortiumId, ConsortiumEntity> Consortia = new Dictionary<ConsortiumId, ConsortiumEntity>();

    public Task<ConsortiumEntity> GetConsortium(ConsortiumId consortiumId)
    {
        if (Consortia.TryGetValue(consortiumId, out var consortium))
        {
            return Task.FromResult(consortium);
        }

        throw new NotFoundException("Consortium", consortiumId.Value);
    }

    public Task<ConsortiumEntity> Save(ConsortiumEntity consortiumEntity)
    {
        if (consortiumEntity.Id.IsNew)
        {
            consortiumEntity.SetId(new ConsortiumId(Guid.NewGuid().ToString()));
            Consortia[consortiumEntity.Id] = consortiumEntity;
        }

        return Task.FromResult(consortiumEntity);
    }

    public Task<bool> IsPartOfConsortiumForProgramme(ProgrammeId programmeId, OrganisationId organisationId)
    {
        return Task.FromResult(false);
    }
}
