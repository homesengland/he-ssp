using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Models.HomeTypes;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class DisabledPeopleClientGroupTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/DisabledPeopleClientGroup.cshtml";

    private static readonly DisabledPeopleClientGroupModel Model = new("My application", "My homes");

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp when using AngleSharp")]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasPageHeader("My application - My homes", "What client group are the homes for?")
            .HasRadio(
                "DisabledPeopleClientGroup",
                new[]
                {
                    "PeopleAtRiskOfDomesticViolence",
                    "PeopleWithAlcoholProblems",
                    "PeopleWithDrugProblems",
                    "PeopleWithHivOrAids",
                    "PeopleWithLearningDisabilitiesOrAutism",
                    "PeopleWithMentalHealthProblems",
                    "PeopleWithMultipleComplexNeeds",
                    "PeopleWithPhysicalOrSensoryDisabilities",
                    "MilitaryVeteransWithSupportNeeds",
                    "OffendersAndPeopleAtRiskOfOffending",
                    "HomelessFamiliesWithSupportNeeds",
                    "Refugees",
                    "RoughSleepers",
                    "SingleHomelessPeopleWithSupportNeeds",
                    "TeenageParents",
                    "YoungPeopleAtRisk",
                    "YoungPeopleLeavingCare",
                })
            .HasGdsSaveAndContinueButton();
    }
}
