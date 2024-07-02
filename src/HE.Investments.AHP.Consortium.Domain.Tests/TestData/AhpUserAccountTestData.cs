using HE.Investments.Common.Contract;
using HE.Investments.Common.Tests.TestData;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investments.AHP.Consortium.Domain.Tests.TestData;

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

    public static readonly ConsortiumUserAccount UserAccountOneWithConsortiumAsMember = new(
        UserAccountTestData.UserAccountOne.UserGlobalId,
        UserAccountTestData.UserAccountOne.UserEmail,
        UserAccountTestData.UserAccountOne.Organisation,
        UserAccountTestData.UserAccountOne.Roles,
        new ConsortiumBasicInfo(new ConsortiumId("consortium-id-123"), false, []));
}
