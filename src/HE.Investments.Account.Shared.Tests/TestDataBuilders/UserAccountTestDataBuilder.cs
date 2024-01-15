using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.TestsUtils;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investments.Account.Shared.Tests.TestDataBuilders;

public class UserAccountTestDataBuilder : TestObjectBuilder<UserAccount>
{
    public UserAccountTestDataBuilder()
    {
        Item = new UserAccount(
            UserGlobalId.From("user-1"),
            "test@email.com",
            new OrganisationBasicInfo(new OrganisationId("00000000-0000-0000-0000-000000000001"), false),
            "Organisation 1",
            new List<UserRole>());
    }

    public UserAccountTestDataBuilder WithUserGlobalId(string userGlobalId)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(Item, nameof(Item.UserGlobalId), UserGlobalId.From(userGlobalId));
        return this;
    }

    public UserAccountTestDataBuilder WithOrganisationId(string organisationId)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(Item, nameof(Item.Organisation), new OrganisationBasicInfo(new OrganisationId(organisationId), false));
        return this;
    }
}
