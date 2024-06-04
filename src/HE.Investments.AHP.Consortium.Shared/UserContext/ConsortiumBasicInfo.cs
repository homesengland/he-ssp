using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.Consortium.Shared.UserContext;

public record ConsortiumBasicInfo(ConsortiumId ConsortiumId, bool IsLeadPartner, IList<OrganisationId> ActiveMembers)
{
    public bool HasNoConsortium => ConsortiumId == ConsortiumId.From(Guid.Empty.ToString());

    public static ConsortiumBasicInfo NoConsortium => new(ConsortiumId.From(Guid.Empty.ToString()), false, []);

    public string? GetConsortiumIdAsString() => HasNoConsortium ? null : ConsortiumId.Value;
}
