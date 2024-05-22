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
    private readonly DataManipulator _dataManipulator;

    public CreateProjectForAhp(FrontDoorIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
        _dataManipulator = fixture.ServiceProvider.GetRequiredService<DataManipulator>();
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(1)]
    public async Task ProjectShouldShouldBeEligibilityForAhp()
    {
        // given
        var userAccount = new UserAccount(
            UserGlobalId.From(LoginData.UserGlobalId),
            LoginData.Email,
            new OrganisationBasicInfo(
                OrganisationId.From(LoginData.OrganisationId),
                string.Empty,
                string.Empty,
                string.Empty,
                false),
            [UserRole.Admin]);

        var projectId = await _dataManipulator.CreateProjectEligibleForAhp(userAccount);
        var currentPage = await TestClient.NavigateTo(ProjectPagesUrl.CheckAnswers(projectId));

        // when
        var continueButton = currentPage
            .UrlEndWith(ProjectPagesUrl.CheckAnswers(projectId))
            .HasTitle(ProjectPageTitles.CheckAnswers)
            .GetSubmitButton("Accept and submit");

        var nextPage = await TestClient.SubmitButton(continueButton);

        // then
        nextPage.ToHtml().Should().Contain("Eligibility for Ahp");
        Output.WriteLine(nextPage.Body?.GetDescendants()?.FirstOrDefault()?.TextContent);
    }
}
