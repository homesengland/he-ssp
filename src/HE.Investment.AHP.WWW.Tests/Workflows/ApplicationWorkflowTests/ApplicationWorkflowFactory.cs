using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.WWW.Tests.Workflows.ApplicationWorkflowTests;

public static class ApplicationWorkflowFactory
{
    private static readonly List<ApplicationSection> NotStartedSections = new()
    {
        new(SectionType.Scheme, SectionStatus.NotStarted),
        new(SectionType.HomeTypes, SectionStatus.NotStarted),
        new(SectionType.FinancialDetails, SectionStatus.NotStarted),
        new(SectionType.DeliveryPhases, SectionStatus.NotStarted),
    };

    public static ApplicationWorkflow BuildWorkflow(
        ApplicationWorkflowState currentSiteWorkflowState,
        string name = "new application",
        Tenure tenure = Tenure.Undefined,
        ApplicationStatus status = ApplicationStatus.Draft,
        string? referenceNumber = null,
        ModificationDetails? lastModificationDetails = null,
        IList<ApplicationSection>? sections = null,
        IEnumerable<AhpApplicationOperation>? allowedOperations = null)
    {
        var application = new Application(
            new AhpApplicationId("new id"),
            name,
            tenure,
            status,
            allowedOperations?.ToList() ?? new List<AhpApplicationOperation>(),
            referenceNumber,
            lastModificationDetails,
            null,
            sections ?? NotStartedSections);

        return new ApplicationWorkflow(
            currentSiteWorkflowState,
            () => Task.FromResult(application),
            () => Task.FromResult(true),
            application.IsReadOnly);
    }
}
