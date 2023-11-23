using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.Tests.WWW.Helpers;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class RevenueFundingTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/RevenueFunding.cshtml";

    [Fact]
    public async Task ShouldRenderViewWithCheckboxes()
    {
        // given
        var model = new RevenueFundingModel("My application", "My homes");

        // when
        var document = await RenderHomeTypePage(ViewPath, model);

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

    [Fact]
    public async Task ShouldRenderViewWithCheckedCheckboxes()
    {
        // given
        var model = new RevenueFundingModel("My application", "My homes")
        {
            Sources = new List<RevenueFundingSourceType>
            {
                RevenueFundingSourceType.SocialServicesDepartment,
                RevenueFundingSourceType.Charity,
                RevenueFundingSourceType.NationalLottery,
            },
        };

        // when
        var document = await RenderHomeTypePage(ViewPath, model);

        // then
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "Where are you receiving revenue funding from for these homes?")
            .HasElementWithText("span", "Select all that apply")
            .HasCheckedCheckboxes(
                "Sources",
                new[]
                {
                    "Charity",
                    "NationalLottery",
                    "SocialServicesDepartment",
                })
            .HasElementWithText("button", "Save and continue");
    }
}
