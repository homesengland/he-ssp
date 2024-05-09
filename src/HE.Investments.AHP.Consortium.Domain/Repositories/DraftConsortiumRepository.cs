extern alias Org;

using HE.Investments.Account.Shared.User;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;

namespace HE.Investments.AHP.Consortium.Domain.Repositories;

public class DraftConsortiumRepository : IDraftConsortiumRepository
{
    private readonly ICacheService _cache;

    public DraftConsortiumRepository(ICacheService cache)
    {
        _cache = cache;
    }

    public DraftConsortiumEntity? Get(ConsortiumId consortiumId, UserAccount userAccount, bool throwException = false)
    {
        var consortium = _cache.GetValue<DraftConsortiumDto>(StorageKey(consortiumId, userAccount));
        if (consortium.IsNotProvided() && throwException)
        {
            throw new NotFoundException("Consortium", consortiumId.Value);
        }

        return ToEntity(consortium);
    }

    public void Create(ConsortiumEntity consortium, UserAccount userAccount)
    {
        var draftConsortium = new DraftConsortiumEntity(
            consortium.Id,
            consortium.Name,
            consortium.Programme,
            new DraftConsortiumMember(consortium.LeadPartner.Id, consortium.LeadPartner.OrganisationName),
            new List<DraftConsortiumMember>());

        _cache.SetValue(StorageKey(consortium.Id, userAccount), ToDto(draftConsortium));
    }

    public void Save(DraftConsortiumEntity consortium, UserAccount userAccount)
    {
        _cache.SetValue(StorageKey(consortium.Id, userAccount), ToDto(consortium));
    }

    public void Delete(DraftConsortiumEntity consortium, UserAccount userAccount)
    {
        _cache.Delete(StorageKey(consortium.Id, userAccount));
    }

    private static DraftConsortiumDto ToDto(DraftConsortiumEntity entity)
    {
        return new DraftConsortiumDto(
            entity.Id.Value,
            entity.Name.Value,
            entity.Programme.Id.Value,
            entity.Programme.Name,
            new DraftConsortiumMemberDto(entity.LeadPartner.Id.Value, entity.LeadPartner.OrganisationName),
            entity.Members.Select(x => new DraftConsortiumMemberDto(x.Id.Value, x.OrganisationName)).ToList());
    }

    private static DraftConsortiumEntity? ToEntity(DraftConsortiumDto? dto)
    {
        if (dto.IsNotProvided())
        {
            return null;
        }

        return new DraftConsortiumEntity(
            new ConsortiumId(dto!.Id),
            new ConsortiumName(dto.Name),
            new ProgrammeSlim(new ProgrammeId(dto.ProgrammeId), dto.ProgrammeName),
            new DraftConsortiumMember(new OrganisationId(dto.LeadPartner.Id), dto.LeadPartner.Name),
            dto.Members.Select(x => new DraftConsortiumMember(new OrganisationId(x.Id), x.Name)).ToList());
    }

    private static string StorageKey(ConsortiumId consortiumId, UserAccount userAccount) => $"consortium-{consortiumId}-{userAccount.UserGlobalId}";

    private sealed record DraftConsortiumDto(
        string Id,
        string Name,
        string ProgrammeId,
        string ProgrammeName,
        DraftConsortiumMemberDto LeadPartner,
        IList<DraftConsortiumMemberDto> Members);

    private sealed record DraftConsortiumMemberDto(string Id, string Name);
}
