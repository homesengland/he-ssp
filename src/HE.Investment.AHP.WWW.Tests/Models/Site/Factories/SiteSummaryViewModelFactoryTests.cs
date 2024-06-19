using FluentAssertions;
using HE.Investment.AHP.WWW.Models.Site.Factories;
using HE.Investment.AHP.WWW.Tests.TestDataBuilders;
using HE.Investments.Common.WWW.Components.SectionSummary;
using HE.Investments.TestsUtils.TestFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Moq;

namespace HE.Investment.AHP.WWW.Tests.Models.Site.Factories;

public class SiteSummaryViewModelFactoryTests : TestBase<SiteSummaryViewModelFactory>
{
    [Fact]
    public void ShouldReturnConsortiumSection_WhenOrganisationIsInConsortium()
    {
        // given
        var site = SiteModelBuilder.Build(
            id: "1",
            developingPartner: new OrganisationDetailsBuilder().WithName("My developing partner").Build(),
            ownerOfTheLand: new OrganisationDetailsBuilder().WithName("My owner of the land").Build(),
            ownerOfTheHomes: new OrganisationDetailsBuilder().WithName("My owner of the homes").Build(),
            isConsortiumMember: true,
            isUnregisteredBody: true);

        // when
        var result = TestCandidate.CreateSiteSummary(site, MockUrlHelper(), true, false)
            .ToDictionary(x => x.Title, x => x.Items);

        // then
        result.Should().ContainKey("Consortium");
        result.Should().NotContainKey("URB");
        result["Consortium"]
            .Should()
            .BeEquivalentTo(
                new[]
                {
                    new SectionSummaryItemModel("Developing partner", ["My developing partner"], "SitePartners/DevelopingPartner"),
                    new SectionSummaryItemModel("Owner of the land", ["My owner of the land"], "SitePartners/OwnerOfTheLand"),
                    new SectionSummaryItemModel("Owner of the homes", ["My owner of the homes"], "SitePartners/OwnerOfTheHomes"),
                });
    }

    [Fact]
    public void ShouldReturnUnregisteredBodySection_WhenOrganisationIsUrbNotInConsortium()
    {
        // given
        var site = SiteModelBuilder.Build(
            id: "1",
            ownerOfTheHomes: new OrganisationDetailsBuilder().WithName("My owner of the homes").Build(),
            isConsortiumMember: false,
            isUnregisteredBody: true);

        // when
        var result = TestCandidate.CreateSiteSummary(site, MockUrlHelper(), true, false)
            .ToDictionary(x => x.Title, x => x.Items);

        // then
        result.Should().NotContainKey("Consortium");
        result.Should().ContainKey("URB");
        result["URB"]
            .Should()
            .BeEquivalentTo(new[] { new SectionSummaryItemModel("Owner of the homes", ["My owner of the homes"], "SitePartners/UnregisteredBodySearch") });
    }

    private static IUrlHelper MockUrlHelper()
    {
        var urlHelper = new Mock<IUrlHelper>();
        var actionContext = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
        };

        urlHelper.Setup(x => x.ActionContext).Returns(actionContext);

        urlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
            .Returns<UrlActionContext>(context =>
            {
                var queryParameters = context.Values!.GetType()
                    .GetProperties()
                    .Where(x => x.GetValue(context.Values, null) != null)
                    .ToDictionary(
                        x => x.Name,
                        x => x.GetValue(context.Values, null)!.ToString());

                return QueryHelpers.AddQueryString($"{context.Controller}/{context.Action}", queryParameters);
            });

        return urlHelper.Object;
    }
}
