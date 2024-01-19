using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Models.Delivery;
using HE.Investment.AHP.WWW.Views.Delivery.Const;
using Microsoft.AspNetCore.Routing;

namespace HE.Investment.AHP.WWW.Tests.Views.DeliveryPhase;

public class AddHomesTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/DeliveryPhase/AddHomes.cshtml";

    private readonly RouteData _routeData = new(new RouteValueDictionary { { "applicationId", "123" }, { "deliveryPhaseId", "321" } });

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Assertion present")]
    public async Task ShouldDisplayView()
    {
        // given
        var homeTypes = new Dictionary<string, string> { { "ht-1", "First Home Type" }, { "ht-2", "Second Home Type" } };
        var homesToDeliver = new Dictionary<string, string?> { { "ht-1", "5" }, { "ht-2", null } };
        var model = new AddHomesModel(
            "123",
            "My application",
            "Phase one",
            homeTypes,
            homesToDeliver);

        // when
        var document = await Render(_viewPath, model, routeData: _routeData);

        // then
        document.HasPageHeader("My application - Phase one", DeliveryPageTitles.AddHomes)
            .HasParagraph("Enter the number of homes from each home type that will be delivered in this phase.")
            .HasParagraph("If a home type is not being delivered in this phase, enter 0.")
            .HasParagraph("Once a home type has been fully allocated to your delivery phases, it will no longer be listed.")
            .HasInput("HomesToDeliver[ht-1]", "First Home Type", "5")
            .HasInput("HomesToDeliver[ht-2]", "Second Home Type")
            .HasGdsSaveAndContinueButton()
            .HasGdsBackButton(false);
    }
}
