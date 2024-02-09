using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.WWW.Models;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class DesignPlansTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/DesignPlans.cshtml";

    private static readonly DesignPlansModel Model = new("My application", "My homes")
    {
        MoreInformation = "Some details about my Design Plans",
        UploadedFiles = new[] { new FileModel("file-1", "My File", new DateTime(2022, 10, 11, 0, 0, 0, DateTimeKind.Unspecified), "Test User", true, "#", "#") },
        MaxFileSizeInMegabytes = 20,
        AllowedExtensions = "JPG, PDF",
    };

    [Fact]
    public async Task ShouldRenderViewListOfUploadedFiles()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasPageHeader("My application - My homes", "Upload your design plans")
            .HasElementWithText("span", "Upload a file (JPG, PDF)")
            .HasElementWithText("span", "Maximum file size 20 MB")
            .HasInput("File")
            .HasElementWithText("a", "My File")
            .HasElementWithText("td", "uploaded 11/10/2022 01:00:00 by Test User")
            .HasElementWithText("label", "Tell us more about your design plans (optional)")
            .HasElementWithText("div", "Tell us any important information about the plans, or any additional information not included.")
            .HasTextAreaInput("MoreInformation", value: "Some details about my Design Plans")
            .HasSaveAndContinueButton();
    }

    [Fact]
    public async Task ShouldDisplayView_WhenThereIsFileError()
    {
        // given
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("File", ErrorMessage);

        // when
        var document = await RenderHomeTypePage(ViewPath, Model, modelStateDictionary: modelState);

        // then
        AssertErrors(document, "File", true);
    }
}
