using HE.Investments.Account.IntegrationTests.Data;
using HE.Investments.Account.WWW;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.IntegrationTestsFramework.Auth;
using HE.Investments.IntegrationTestsFramework.Config;
using Xunit;

namespace HE.Investments.Account.IntegrationTests.Framework;

[Collection(nameof(AccountIntegrationTestSharedContext))]
public class AccountIntegrationTest : IntegrationTestBase<Program>
{
    protected AccountIntegrationTest(IntegrationTestFixture<Program> fixture, bool createFreshAccount = false)
        : base(fixture)
    {
        if (createFreshAccount)
        {
            StoreLoginData(fixture.LoginData);
            fixture.ProvideLoginData(GenerateFreshAccount());
        }
        else
        {
            var loginData = RestoreLoginData();
            if (loginData != null)
            {
                fixture.ProvideLoginData(loginData);
            }
        }

        SetFreshProfileData();
        SetUserOrganisationData();
        fixture.CheckUserLoginData();
        TestClient.AsLoggedUser();
    }

    public FreshProfileData FreshProfileData { get; private set; }

    public UserOrganisationData UserOrganisationData { get; private set; }

    private UserLoginData GenerateFreshAccount()
    {
        var userLoginData = GetSharedDataOrNull<UserLoginData>("FreshUserData");
        if (userLoginData is null)
        {
            var userGlobalId = $"itests|{Guid.NewGuid()}";
            userLoginData = new UserLoginData
            {
                UserGlobalId = userGlobalId,
                Email = $"{userGlobalId}@integrationTests.it",
                OrganisationId = Guid.Empty.ToString(),
            };

            SetSharedData("FreshUserData", userLoginData);
        }

        return userLoginData;
    }

    private void StoreLoginData(ILoginData loginData)
    {
        var userLoginData = GetSharedDataOrNull<ILoginData>("OriginalLoginData");
        if (userLoginData is null)
        {
            SetSharedData(
                "OriginalLoginData",
                new UserLoginData { UserGlobalId = loginData.UserGlobalId, Email = loginData.Email, OrganisationId = loginData.OrganisationId, });
        }
    }

    private ILoginData? RestoreLoginData()
    {
        return GetSharedDataOrNull<ILoginData>("OriginalLoginData");
    }

    private void SetFreshProfileData()
    {
        var freshProfileData = GetSharedDataOrNull<FreshProfileData>(nameof(FreshProfileData));
        if (freshProfileData is null)
        {
            freshProfileData = new FreshProfileData();
            SetSharedData(nameof(FreshProfileData), freshProfileData);
        }

        FreshProfileData = freshProfileData;
    }

    private void SetUserOrganisationData()
    {
        var userOrganisationData = GetSharedDataOrNull<UserOrganisationData>(nameof(UserOrganisationData));
        if (userOrganisationData is null)
        {
            userOrganisationData = new UserOrganisationData();
            SetSharedData(nameof(UserOrganisationData), userOrganisationData);
        }

        UserOrganisationData = userOrganisationData;
    }
}
