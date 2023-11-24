using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;
internal static class PlanningReferenceNumberTestData
{
    public static readonly PlanningReferenceNumber ExistingReferenceNumber = new(true, "anyNumber");

    public static readonly PlanningReferenceNumber NonExistingReferenceNumber = new(false, string.Empty);
}
