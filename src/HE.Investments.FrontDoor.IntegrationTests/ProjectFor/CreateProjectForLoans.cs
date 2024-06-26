using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.Investments.FrontDoor.IntegrationTests.Framework;
using HE.Investments.FrontDoor.IntegrationTests.Pages;
using HE.Investments.FrontDoor.WWW.Views.Project.Const;
using HE.Investments.IntegrationTestsFramework.Assertions;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.FrontDoor.IntegrationTests.ProjectFor;

[Order(1)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class CreateProjectForLoans : FrontDoorIntegrationTest
{
    public CreateProjectForLoans(FrontDoorIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(1)]
    public async Task ProjectShouldBeEligibleForLoans()
    {
        // given
        var (projectId, _) = await InFrontDoor.FrontDoorProjectEligibleForLoansExist(LoginData);
        var currentPage = await TestClient.NavigateTo(ProjectPagesUrl.CheckAnswers(UserOrganisationData.OrganisationId, projectId));

        // when
        var continueButton = currentPage
            .UrlEndWith(ProjectPagesUrl.CheckAnswers(UserOrganisationData.OrganisationId, projectId))
            .HasTitle(ProjectPageTitles.CheckAnswers)
            .GetSubmitButton("Accept and submit");

        var nextPage = await TestClient.SubmitButton(continueButton);

        // then
        var displayedText = nextPage.GetSummaryListItems();
        displayedText.Should().ContainKey("Eligible programme").WithValue("Loans");

        Output.WriteLine($"Create Loans Project link: {displayedText["Eligible programme"].ChangeAnswerLink?.Href}");
    }
}
