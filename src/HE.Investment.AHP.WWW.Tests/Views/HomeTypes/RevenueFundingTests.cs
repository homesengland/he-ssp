using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.Tests.WWW.Helpers;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class RevenueFundingTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/RevenueFunding.cshtml";

    private static readonly RevenueFundingModel Model = new("My application", "My homes");

    [Fact]
    public async Task ShouldRenderViewWithCheckboxes()
    {
        // given & when
        var document = await Render(ViewPath, Model);

        // then
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "Where are you receiving revenue funding from for these homes?")
            .HasElementWithText("span", "Select all that apply")
            .HasCheckboxes(
                "Sources",
                new[]
                {
                    "Charity",
                    "ClinicalCommissioningGroupLocalAreaTeam",
                    "CrimeAndDisorderReductionPartnerships",
                    "DepartmentForEducation",
                    "DrugsActionTeam",
                    "HealthAndWellBeingBoard",
                    "HomeOffice",
                    "HousingDepartment",
                    "LocalAreaAgreements",
                    "NationalLottery",
                    "NhsEngland",
                    "NhsTrust",
                    "OtherHealthSource",
                    "OtherLocalAuthoritySource",
                    "ProbationService",
                    "ProvidersReserves",
                    "SocialServicesDepartment",
                    "SupportingPeople",
                    "YouthOffendingTeams",
                    "Other",
                })
            .HasElementWithText("button", "Save and continue");
    }
}
