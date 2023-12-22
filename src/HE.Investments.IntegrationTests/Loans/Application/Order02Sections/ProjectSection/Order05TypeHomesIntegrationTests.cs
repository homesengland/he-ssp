using System.Diagnostics.CodeAnalysis;
using HE.Investments.Common.Messages;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.IntegrationTestsFramework.Extensions;
using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.Loans.WWW.Views.Project.Consts;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.ProjectSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order05TypeHomesIntegrationTests : IntegrationTest
{
    private readonly string _projectId;

    private readonly string _applicationLoanId;

    public Order05TypeHomesIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
        _projectId = UserData.ProjectInDraftStateId;
    }

    [Order(1)]
    [Fact(Skip = LoansConfig.SkipTest)]
    public async Task Order01_ShouldDisplayValidationMessage_WhenOtherTypeIsSelectedButNoNameIsProvided()
    {
        // given
        var typeHomesPage = await TestClient.NavigateTo(ProjectPagesUrls.TypeHomes(_applicationLoanId, _projectId));
        var continueButton = typeHomesPage.GetGdsSubmitButtonById("continue-button");

        // when
        typeHomesPage = await TestClient.SubmitButton(
            continueButton,
            ("HomeTypes", "other"),
            ("OtherHomeTypes", string.Empty));

        // then
        typeHomesPage
            .UrlEndWith(ProjectPagesUrls.TypeHomesSuffix)
            .HasTitle(ProjectPageTitles.TypeHomes)
            .HasOneValidationMessages(ValidationErrorMessage.TypeHomesOtherType);

        SetCurrentPage(typeHomesPage);
    }

    [Order(2)]
    [Fact(Skip = LoansConfig.SkipTest)]
    public async Task Order02_ShouldDisplayValidationMessage_WhenOtherTypeIsSelectedButProvidedNameIsTooLong()
    {
        // given
        var typeHomesPage = await GetCurrentPage(ProjectPagesUrls.TypeHomes(_applicationLoanId, _projectId));
        var continueButton = typeHomesPage.GetGdsSubmitButtonById("continue-button");

        // when
        typeHomesPage = await TestClient.SubmitButton(
            continueButton,
            ("HomeTypes", "other"),
            ("OtherHomeTypes", TextTestData.TextThatExceedsShortInputLimit));

        // then
        typeHomesPage
            .UrlEndWith(ProjectPagesUrls.TypeHomesSuffix)
            .HasTitle(ProjectPageTitles.TypeHomes)
            .HasOneValidationMessages(ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.OtherHomeType));
    }

    [Order(3)]
    [Fact(Skip = LoansConfig.SkipTest)]
    public async Task Order03_ShouldRedirectToProjectType_WhenOtherTypeIsSelectedAndCorrectNameIsProvided()
    {
        // given
        var typeHomesPage = await GetCurrentPage(ProjectPagesUrls.TypeHomes(_applicationLoanId, _projectId));
        var continueButton = typeHomesPage.GetGdsSubmitButtonById("continue-button");

        // when
        var projectType = await TestClient.SubmitButton(
            continueButton,
            ("HomeTypes", "other"),
            ("OtherHomeTypes", TextTestData.TextThatNotExceedsShortInputLimit));

        // then
        projectType
            .UrlEndWith(ProjectPagesUrls.ProjectTypeSuffix)
            .HasTitle(ProjectPageTitles.ProjectType);
    }
}
