using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common;
using HE.Investment.AHP.Contract.Scheme;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Common.Contract;
using HE.Investments.Organisation.Contract;

namespace HE.Investment.AHP.WWW.Tests.Workflows.SchemeWorkflowTests;

public static class SchemeWorkflowFactory
{
    public static SchemeWorkflow BuildWorkflow(
        SchemeWorkflowState currentSchemeWorkflowState,
        ApplicationDetails? application = null,
        SectionStatus status = SectionStatus.Undefined,
        decimal? requiredFunding = null,
        int? housesToDeliver = null,
        OrganisationDetails? developingPartner = null,
        OrganisationDetails? ownerOfTheLand = null,
        OrganisationDetails? ownerOfTheHomes = null,
        bool? arePartnersConfirmed = null,
        string? affordabilityEvidence = null,
        string? salesRisk = null,
        string? meetingLocalPriorities = null,
        string? meetingLocalHousingNeed = null,
        string? stakeholderDiscussionsReport = null,
        UploadedFile? localAuthoritySupportFile = null,
        bool isConsortiumMember = false)
    {
        var scheme = new Scheme(
            application ?? new ApplicationDetails(
                new AhpApplicationId("test-1234"),
                "appName",
                Tenure.AffordableRent,
                ApplicationStatus.Draft,
                [AhpApplicationOperation.Modification]),
            status,
            requiredFunding,
            housesToDeliver,
            developingPartner,
            ownerOfTheLand,
            ownerOfTheHomes,
            arePartnersConfirmed,
            affordabilityEvidence,
            salesRisk,
            meetingLocalPriorities,
            meetingLocalHousingNeed,
            stakeholderDiscussionsReport,
            localAuthoritySupportFile,
            isConsortiumMember);

        return new SchemeWorkflow(currentSchemeWorkflowState, scheme);
    }
}
