using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.Common.Tests.TestData;

namespace HE.Investment.AHP.Domain.Tests.Common.TestData;

public static class AhpUserAccountTestData
{
    public static readonly AhpUserAccount UserAccountOneNoConsortium = new(
        UserAccountTestData.UserAccountOne.UserGlobalId,
        UserAccountTestData.UserAccountOne.UserEmail,
        UserAccountTestData.UserAccountOne.Organisation,
        UserAccountTestData.UserAccountOne.Roles,
        AhpConsortiumBasicInfo.NoConsortium);

    public static readonly AhpUserAccount UserAccountOneWithConsortium = new(
        UserAccountTestData.UserAccountOne.UserGlobalId,
        UserAccountTestData.UserAccountOne.UserEmail,
        UserAccountTestData.UserAccountOne.Organisation,
        UserAccountTestData.UserAccountOne.Roles,
        new AhpConsortiumBasicInfo(new ConsortiumId("consortium-id-123"), true, []));
}
