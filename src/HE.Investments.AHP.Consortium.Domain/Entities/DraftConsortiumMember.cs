using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.AHP.Consortium.Domain.ValueObjects;
using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.Consortium.Domain.Entities;

public record DraftConsortiumMember(OrganisationId Id, string OrganisationName) : IConsortiumMember
{
    public ConsortiumMemberStatus Status => ConsortiumMemberStatus.Active;
}
