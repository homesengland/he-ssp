using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.AHP.Consortium.Domain.ValueObjects;
using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.Consortium.Domain.Tests.TestData;

public static class ConsortiumMemberTestData
{
    public static readonly ConsortiumMember CarqMember = new(
        new OrganisationId("e89efe2b-1df8-4e67-a10a-867bf0f85913"),
        "Carq Consortium Member",
        ConsortiumMemberStatus.Active);
}
