using HE.InvestmentLoans.IntegrationTests.Config;
using HE.InvestmentLoans.IntegrationTests.Loans;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace HE.InvestmentLoans.IntegrationTests.IntegrationFramework;

[Collection(nameof(IntegrationTestSharedContext))]
public class IntegrationTest
{
    private readonly IntegrationTestFixture<Program> _fixture;

    protected IntegrationTest(IntegrationTestFixture<Program> fixture)
    {
        _fixture = fixture;

        var configuration = CreateConfiguration();

        var userConfig = GetUserConfigFrom(configuration);

        TestClient = new IntegrationTestClient(fixture.CreateClient(), new IntegrationTestConfig(userConfig));
    }

    protected IntegrationTestClient TestClient { get; }

    protected void SetSharedData<T>(string key, T data)
        where T : notnull
    {
        _fixture.DataBag[key] = data;
    }

    protected T GetSharedData<T>(string key)
        where T : class
    {
#if DEBUG
        if (key == SharedKeys.ApplicationLoanIdInDraftStatusKey)
        {
            return ("0a440401-dc52-ee11-be6f-002248c653e1" as T)!;
        }
#endif

        if (!_fixture.DataBag.ContainsKey(key))
        {
            return null!;
        }

        return (_fixture.DataBag[key] as T)!;
    }

    private static IConfigurationRoot CreateConfiguration()
    {
        return new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile("appsettings.Development.json", true)
                    .Build();
    }

    private static UserConfig GetUserConfigFrom(IConfigurationRoot configuration)
    {
        var userSection = configuration.GetSection("User");

        var userConfig = new UserConfig(
            userSection["UserGlobalId"]!,
            userSection["Email"]!,
            userSection["OrganizationName"]!,
            userSection["OrganizationRegistrationNumber"]!,
            userSection["OrganizationAddress"]!,
            userSection["ContactName"]!,
            userSection["TelephoneNumer"]!);
        return userConfig;
    }

}
