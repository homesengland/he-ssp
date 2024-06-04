using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.AHP.Consortium.Shared.UserContext;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Tests.TestData;

namespace HE.Investment.AHP.Domain.Tests.Common.TestData;

public static class AhpUserAccountTestData
{
    public static readonly ConsortiumUserAccount UserAccountOneNoConsortium = new(
        UserAccountTestData.UserAccountOne.UserGlobalId,
        UserAccountTestData.UserAccountOne.UserEmail,
        UserAccountTestData.UserAccountOne.Organisation,
        UserAccountTestData.UserAccountOne.Roles,
        ConsortiumBasicInfo.NoConsortium);

    public static readonly ConsortiumUserAccount UserAccountOneWithConsortium = new(
        UserAccountTestData.UserAccountOne.UserGlobalId,
        UserAccountTestData.UserAccountOne.UserEmail,
        UserAccountTestData.UserAccountOne.Organisation,
        UserAccountTestData.UserAccountOne.Roles,
        new ConsortiumBasicInfo(new ConsortiumId("consortium-id-123"), true, []));
}
