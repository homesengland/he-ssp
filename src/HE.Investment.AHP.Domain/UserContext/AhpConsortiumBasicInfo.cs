using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.UserContext;

public record AhpConsortiumBasicInfo(ConsortiumId ConsortiumId, bool IsLeadPartner, IList<OrganisationId> ActiveMembers)
{
    public bool HasNoConsortium => ConsortiumId == ConsortiumId.From(Guid.Empty.ToString());

    public static AhpConsortiumBasicInfo NoConsortium => new(ConsortiumId.From(Guid.Empty.ToString()), false, []);
}
