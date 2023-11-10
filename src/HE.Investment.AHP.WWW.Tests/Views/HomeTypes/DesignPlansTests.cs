using HE.Investment.AHP.WWW.Models.Common;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investment.AHP.WWW.Tests.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class DesignPlansTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/DesignPlans.cshtml";

    private static readonly DesignPlansModel Model = new("My application", "My homes")
    {
        MoreInformation = "Some details about my Design Plans",
        UploadedFiles = new[] { new UploadedFileModel("file-1", "My File", new DateTime(2022, 10, 11), "Test User", true) },
    };

    [Fact]
    public async Task ShouldRenderViewListOfUploadedFiles()
    {
        // given & when
        var document = await Render(ViewPath, Model);

        // then
        document
            .HasElementWithText("div", "My application - My homes")
            .HasElementWithText("h1", "Upload your design plans")
            .HasInput("File")
            .HasElementWithText("td", "My File")
            .HasElementWithText("td", "uploaded 11/10/2022 01:00:00 by Test User")
            .HasElementWithText("label", "Tell us more about your design plans (optional)")
            .HasElementWithText("div", "Tell us any important information about the plans, or any additional information not included.")
            .HasInput("MoreInformation", value: "Some details about my Design Plans")
            .HasElementWithText("button", "Save and continue");
    }

    [Fact]
    public async Task ShouldDisplayView_WhenThereIsFileError()
    {
        // given
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("File", ErrorMessage);

        // when
        var document = await Render(ViewPath, Model, modelStateDictionary: modelState);

        // then
        AssertErrors(document, "File", true);
    }
}
