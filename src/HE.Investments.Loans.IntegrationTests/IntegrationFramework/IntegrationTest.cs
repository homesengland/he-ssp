using AngleSharp.Html.Dom;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.IntegrationTestsFramework.Auth;
using HE.Investments.IntegrationTestsFramework.Config;
using HE.Investments.Loans.IntegrationTests.Config;
using HE.Investments.Loans.IntegrationTests.Crm;
using HE.Investments.Loans.WWW;
using Xunit;

namespace HE.Investments.Loans.IntegrationTests.IntegrationFramework;

[Collection(nameof(IntegrationTestSharedContext))]
public class IntegrationTest : IntegrationTestBase<Program>
{
    private readonly LoansIntegrationTestFixture _fixture;

    protected IntegrationTest(LoansIntegrationTestFixture fixture)
        : base(fixture)
    {
        SetUserData();
        fixture.CheckUserLoginData();
        LoginData = fixture.LoginData;
        fixture.MockUserAccount();
        _fixture = fixture;
        SetUserOrganisationData();
    }

    protected FrontDoorProjectCrmRepository FrontDoorProjectCrmRepository => _fixture.FrontDoorProjectCrmRepository;

    protected LoanApplicationCrmRepository LoanApplicationCrmRepository => _fixture.LoanApplicationCrmRepository;

    protected IntegrationUserData UserData { get; private set; }

    protected ILoginData LoginData { get; private set; }

    public UserOrganisationData UserOrganisationData { get; private set; }

    protected async Task<IHtmlDocument> GetCurrentPage(Func<Task<IHtmlDocument>> alternativeNavigate)
    {
        var currentPage = GetSharedDataOrNull<IHtmlDocument>(CommonKeys.CurrentPageKey);

        if (currentPage is null)
        {
            return await alternativeNavigate();
        }

        return currentPage;
    }

    protected void SetCurrentPage(IHtmlDocument page)
    {
        SetSharedData(CommonKeys.CurrentPageKey, page);
    }

    private void SetUserData()
    {
        var userData = GetSharedDataOrNull<IntegrationUserData>(nameof(IntegrationUserData));
        if (userData is null)
        {
            userData = new IntegrationUserData();
            SetSharedData(nameof(IntegrationUserData), userData);
        }

        UserData = userData;
    }

    private void SetUserOrganisationData()
    {
        var userOrganisationData = GetSharedDataOrNull<UserOrganisationData>(nameof(UserOrganisationData));
        if (userOrganisationData is null)
        {
            userOrganisationData = new UserOrganisationData();
            userOrganisationData.SetOrganisationId(LoginData.OrganisationId);
            SetSharedData(nameof(UserOrganisationData), userOrganisationData);
        }

        UserOrganisationData = userOrganisationData;
    }
}
