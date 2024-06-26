using HE.Investment.AHP.WWW.Models.HomeTypes;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class PeopleGroupForSpecificDesignFeaturesTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/PeopleGroupForSpecificDesignFeatures.cshtml";

    private static readonly PeopleGroupForSpecificDesignFeaturesModel Model = new("My application", "My homes");

    [Fact]
    public async Task ShouldRenderViewWithRadioButton()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        IList<string> options = [
            "People from ethnic minority backgrounds",
            "Disabled people",
            "Faith groups",
            "People at risk of domestic violence",
            "Young people",
            "Older people",
            "None of these groups",
        ];

        document
            .HasPageHeader("My application - My homes", "Do these homes incorporate specific design features or management arrangements to meet the needs of any of these groups?")
            .HasElementWithText("p", "If these homes are designed specifically to meet the needs of more than one group, select the priority group. You can also create another home type for each group and enter them separately.")
            .HasRadio("PeopleGroupForSpecificDesignFeatures", options)
            .HasSaveAndContinueButton();
    }
}
