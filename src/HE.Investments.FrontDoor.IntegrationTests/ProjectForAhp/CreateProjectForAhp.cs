using System.Diagnostics.CodeAnalysis;
using AngleSharp;
using AngleSharp.Dom;
using FluentAssertions;
using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;
using HE.Investments.FrontDoor.IntegrationTests.Framework;
using HE.Investments.FrontDoor.IntegrationTests.Pages;
using HE.Investments.FrontDoor.WWW.Views.Project.Const;
using HE.Investments.TestsUtils.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.FrontDoor.IntegrationTests.ProjectForAhp;

[Order(1)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class CreateProjectForAhp : FrontDoorIntegrationTest
{
    public CreateProjectForAhp(FrontDoorIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(1)]
    public async Task ProjectShouldBeEligibleForAhp()
    {
        // given
        var (projectId, _) = await InFrontDoor.FrontDoorProjectEligibleForAhpExist(LoginData);
        var currentPage = await TestClient.NavigateTo(ProjectPagesUrl.CheckAnswers(projectId));

        // when
        var continueButton = currentPage
            .UrlEndWith(ProjectPagesUrl.CheckAnswers(projectId))
            .HasTitle(ProjectPageTitles.CheckAnswers)
            .GetSubmitButton("Accept and submit");

        var nextPage = await TestClient.SubmitButton(continueButton);

        // then
        var displayedText = nextPage.GetFirstPreformattedText();
        displayedText.Should().Contain("Eligibility for Ahp");
        Output.WriteLine(displayedText);
    }
}
