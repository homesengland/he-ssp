extern alias Org;

using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Cache;

namespace HE.Investments.AHP.Consortium.Domain.Repositories;

public class DraftConsortiumRepository : IDraftConsortiumRepository
{
    private readonly ITemporaryUserStorage _storage;

    public DraftConsortiumRepository(ITemporaryUserStorage storage)
    {
        _storage = storage;
    }

    public DraftConsortiumEntity? Get(ConsortiumId consortiumId, bool throwException = false)
    {
        var consortium = _storage.GetValue<DraftConsortiumEntity>(StorageKey(consortiumId));
        if (consortium.IsNotProvided() && throwException)
        {
            throw new NotFoundException("Consortium", consortiumId.Value);
        }

        return consortium;
    }

    public void Create(ConsortiumEntity consortium)
    {
        var draftConsortium = new DraftConsortiumEntity(
            consortium.Id.Value,
            consortium.Name.Value,
            consortium.Programme.Id.Value,
            consortium.Programme.Name,
            new DraftConsortiumMember(consortium.LeadPartner.Id, consortium.LeadPartner.OrganisationName),
            new List<DraftConsortiumMember>());

        _storage.SetValue(StorageKey(consortium.Id), draftConsortium);
    }

    public void Save(DraftConsortiumEntity consortium)
    {
        _storage.SetValue(StorageKey(new ConsortiumId(consortium.Id)), consortium);
    }

    public void Delete(DraftConsortiumEntity consortium)
    {
        _storage.Delete(StorageKey(new ConsortiumId(consortium.Id)));
    }

    private static string StorageKey(ConsortiumId consortiumId) => $"consortium-{consortiumId}";
}
