using System.Diagnostics.CodeAnalysis;
using HE.Investments.Common.Messages;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.Loans.WWW.Views.Project.Consts;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.ProjectSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order04ManyHomesIntegrationTests : IntegrationTest
{
    private readonly string _projectId;

    private readonly string _applicationLoanId;

    public Order04ManyHomesIntegrationTests(LoansIntegrationTestFixture fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
        _projectId = UserData.ProjectInDraftStateId;
    }

    [Order(1)]
    [Fact(Skip = LoansConfig.SkipTest)]
    public async Task Order01_ShouldDisplayValidationMessage_WhenIncorrectNumberOfHomesIsProvided()
    {
        // given
        var manyHomesPage = await TestClient.NavigateTo(ProjectPagesUrls.ManyHomes(_applicationLoanId, _projectId));
        var continueButton = manyHomesPage.GetGdsSubmitButtonById("continue-button");

        // when
        manyHomesPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "HomesCount", "asd" } });

        // then
        manyHomesPage
            .UrlEndWith(ProjectPagesUrls.ManyHomesSuffix)
            .HasTitle(ProjectPageTitles.ManyHomes)
            .HasOneValidationMessages(ValidationErrorMessage.ManyHomesAmount);

        SetCurrentPage(manyHomesPage);
    }

    [Order(2)]
    [Fact(Skip = LoansConfig.SkipTest)]
    public async Task Order02_ShouldRedirectToTypeHomes_WhenCorrectNumberOfHomesIsProvided()
    {
        // given
        var manyHomesPage = await TestClient.NavigateTo(ProjectPagesUrls.ManyHomes(_applicationLoanId, _projectId));
        var continueButton = manyHomesPage.GetGdsSubmitButtonById("continue-button");

        // when
        var typeHomesPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "HomesCount", "1" } });

        // then
        typeHomesPage
            .UrlEndWith(ProjectPagesUrls.TypeHomesSuffix)
            .HasTitle(ProjectPageTitles.TypeHomes);

        SetSharedData(SharedKeys.CurrentPageKey, manyHomesPage);
    }
}
