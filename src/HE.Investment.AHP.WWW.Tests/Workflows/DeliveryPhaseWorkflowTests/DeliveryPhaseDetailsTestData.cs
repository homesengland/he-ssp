using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.WWW.Tests.TestDataBuilders;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.WWW.Tests.Workflows.DeliveryPhaseWorkflowTests;

public static class DeliveryPhaseDetailsTestData
{
    public static readonly DeliveryPhaseDetails WithNames = new(ApplicationTestData.SampleApplication, "asd", "PhaseName", SectionStatus.InProgress);
}
