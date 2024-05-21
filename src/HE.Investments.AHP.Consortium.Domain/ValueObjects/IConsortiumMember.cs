using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.Consortium.Domain.ValueObjects;

public interface IConsortiumMember
{
    OrganisationId Id { get; }

    ConsortiumMemberStatus Status { get; }

    string OrganisationName { get; }
}
