using AngleSharp.Html.Dom;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.IntegrationTestsFramework.Auth;
using HE.Investments.IntegrationTestsFramework.Config;
using HE.Investments.Loans.IntegrationTests.Config;
using HE.Investments.Loans.IntegrationTests.Loans;
using HE.Investments.Loans.WWW;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace HE.Investments.Loans.IntegrationTests.IntegrationFramework;

[Collection(nameof(IntegrationTestSharedContext))]
public class IntegrationTest
{
    private readonly IntegrationTestFixture<Program> _fixture;

    protected IntegrationTest(IntegrationTestFixture<Program> fixture)
    {
        _fixture = fixture;
        if (_fixture.DataBag.TryGetValue("UserData", out var userData))
        {
            UserData = (userData as IntegrationUserData)!;
        }
        else
        {
            var userConfig = _fixture.Configuration.GetSection("IntegrationTestsConfig:UserConfig").Get<UserConfig>();
            if (userConfig is not null)
            {
                UserData = new IntegrationUserData(userConfig);
            }
            else
            {
                UserData = new IntegrationUserData();
            }

            _fixture.DataBag["UserData"] = UserData;
        }

        TestClient = new IntegrationTestClient(fixture.CreateClient(), fixture.LoginData);
    }

    protected IntegrationTestClient TestClient { get; }

    protected IntegrationUserData UserData { get; }

    protected ILoginData LoginData => _fixture.LoginData;

    protected void ProvideLoginData(string userGlobalId)
    {
        if (UserData.IsDeveloperProvidedUserData)
        {
            throw new NotSupportedException("Developer provided user data and new user cannot be created");
        }

        _fixture.ProvideLoginData(new UserData()
        {
            UserGlobalId = userGlobalId,
            Email = $"{userGlobalId}@integrationTests.it",
        });
    }

    protected void SetSharedData<T>(string key, T data)
        where T : notnull
    {
        _fixture.DataBag[key] = data;
    }

    protected T GetSharedData<T>(string key)
    {
        return (T)_fixture.DataBag[key];
    }

    protected T? GetSharedDataOrNull<T>(string key)
    {
        return _fixture.DataBag.TryGetValue(key, out var data) ? (T)data : default;
    }

    protected void SetCurrentPage(IHtmlDocument page)
    {
        SetSharedData(SharedKeys.CurrentPageKey, page);
    }

    protected async Task<IHtmlDocument> GetCurrentPage(string navigateTo)
    {
        var currentPage = GetSharedDataOrNull<IHtmlDocument>(SharedKeys.CurrentPageKey);

        if (currentPage is null)
        {
            return await TestClient.NavigateTo(navigateTo);
        }

        return currentPage;
    }

    protected async Task<IHtmlDocument> GetCurrentPage(Func<Task<IHtmlDocument>> alternativeNavigate)
    {
        var currentPage = GetSharedDataOrNull<IHtmlDocument>(SharedKeys.CurrentPageKey);

        if (currentPage is null)
        {
            return await alternativeNavigate();
        }

        return currentPage;
    }
}
