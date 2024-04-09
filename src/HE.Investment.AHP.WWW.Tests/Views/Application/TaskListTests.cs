using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.WWW.Tests.Views.Application;

public class TaskListTests : AhpViewTestBase
{
    private readonly string _viewPath = "/Views/Application/TaskList.cshtml";

    private readonly ModificationDetails _testModificationDetails = new("Jan", "Kowalski", new DateTime(2023, 11, 29, 0, 0, 0, DateTimeKind.Unspecified));

    private readonly IList<ApplicationSection> _testSections = new List<ApplicationSection>
    {
        new(SectionType.Scheme, SectionStatus.Completed), new(SectionType.FinancialDetails, SectionStatus.Completed),
    };

    public static IEnumerable<object[]> AllApplicationStatuses()
    {
        foreach (var number in Enum.GetValues(typeof(ApplicationStatus)))
        {
            yield return new[] { number };
        }
    }

    [Fact]
    public async Task ShouldDisplayView_WhenSectionsAreMissing()
    {
        // given
        var model = CreateApplicationSectionsModel(new List<ApplicationSection>(), _testModificationDetails);

        // when
        var document = await Render(_viewPath, model);

        // then
        AssertView(document, model, false, "You have completed");
    }

    [Fact]
    public async Task ShouldDisplayView_WhenIncompleteSectionExist()
    {
        // given
        var model = CreateApplicationSectionsModel(
            new List<ApplicationSection>
            {
                new(SectionType.Scheme, SectionStatus.NotStarted),
                new(SectionType.HomeTypes, SectionStatus.Completed),
                new(SectionType.FinancialDetails, SectionStatus.InProgress),
            },
            _testModificationDetails);

        // when
        var document = await Render(_viewPath, model);

        // then
        AssertView(document, model, true, "You have completed 1 of 3 sections");
    }

    [Fact]
    public async Task ShouldDisplayView_WhenNoIncompleteSection()
    {
        // given
        var model = CreateApplicationSectionsModel(_testSections, _testModificationDetails);

        // when
        var document = await Render(_viewPath, model);

        // then
        AssertView(document, model, false, "You have completed");
    }

    [Fact]
    public async Task ShouldDisplayView_WithoutModificationDetails_ForMissingData()
    {
        // given
        var model = CreateApplicationSectionsModel(_testSections);

        // when
        var document = await Render(_viewPath, model);

        // then
        AssertView(document, model, false, "You have completed", false);
    }

    [Theory]
    [MemberData(nameof(AllApplicationStatuses))]
    [SuppressMessage("Performance", "CA1825:Avoid zero-length array allocations", Justification = "Tests")]
    public async Task ShouldDisplayView_WithoutModificationDetails_ForStatusesExceptDraftAndReferredBackToApplicant(ApplicationStatus status)
    {
        if (status is ApplicationStatus.Draft or ApplicationStatus.ReferredBackToApplicant)
        {
            return;
        }

        // given
        var model = CreateApplicationSectionsModel(_testSections, _testModificationDetails, status: status);

        // when
        var document = await Render(_viewPath, model);

        // then
        AssertView(document, model, false, "You have completed", false);
    }

    [Fact]
    public async Task ShouldDisplayView_WhenApplicationIsSubmitted()
    {
        // given
        var model = CreateApplicationSectionsModel(_testSections, submissionDetails: _testModificationDetails, status: ApplicationStatus.ApplicationSubmitted);

        // when
        var document = await Render(_viewPath, model);

        // then
        document
            .HasPageHeader(model.SiteName, model.Name)
            .HasElementWithText("p", "Submitted on 29/11/2023 00:00:00 by Jan Kowalski")
            .HasElementWithText("a", "Return to applications");
    }

    private static void AssertView(
        IHtmlDocument document,
        ApplicationSectionsModel sectionsModel,
        bool incompleteSectionsExist,
        string incompleteText,
        bool modificationDetailsExist = true)
    {
        document
            .HasPageHeader(sectionsModel.SiteName, sectionsModel.Name)
            .HasElementWithText("p", incompleteText, incompleteSectionsExist)
            .HasElementWithText("p", "Last saved on 29/11/2023 00:00:00 by Jan Kowalski", modificationDetailsExist)
            .HasElementWithText("p", "You must complete all sections before you can submit your application.", incompleteSectionsExist)
            .HasElementWithText("a", "Return to applications");
    }

    private static ApplicationSectionsModel CreateApplicationSectionsModel(
        IList<ApplicationSection>? sections = null,
        ModificationDetails? modificationDetails = null,
        ModificationDetails? submissionDetails = null,
        ApplicationStatus status = ApplicationStatus.Draft)
    {
        return new ApplicationSectionsModel(
            "A1",
            "some site",
            "application xyz",
            status,
            new[] { AhpApplicationOperation.Modification, AhpApplicationOperation.Submit },
            "Ref1",
            modificationDetails,
            submissionDetails,
            sections ?? new List<ApplicationSection>());
    }
}
