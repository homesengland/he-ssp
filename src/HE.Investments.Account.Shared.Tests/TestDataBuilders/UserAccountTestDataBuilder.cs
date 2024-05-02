using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Common.Contract;
using HE.Investments.TestsUtils;
using HE.Investments.TestsUtils.TestFramework;
using OrganisationId = HE.Investments.Account.Shared.User.ValueObjects.OrganisationId;
using UserAccount = HE.Investments.Account.Shared.User.UserAccount;

namespace HE.Investments.Account.Shared.Tests.TestDataBuilders;

public class UserAccountTestDataBuilder : TestObjectBuilder<UserAccount>
{
    public UserAccountTestDataBuilder()
    {
        Item = new UserAccount(
            UserGlobalId.From("user-1"),
            "test@email.com",
            new OrganisationBasicInfo(new OrganisationId("00000000-0000-0000-0000-000000000001"), "AccountOne", "1234", "London", false),
            new List<UserRole>());
    }

    public UserAccountTestDataBuilder WithUserGlobalId(string userGlobalId)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(Item, nameof(Item.UserGlobalId), UserGlobalId.From(userGlobalId));
        return this;
    }

    public UserAccountTestDataBuilder WithOrganisationId(string organisationId)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(
            Item,
            nameof(Item.Organisation),
            Item.Organisation! with { OrganisationId = new OrganisationId(organisationId) });
        return this;
    }
}
