namespace HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Pages;

public static class AllocationPagesUrl
{
    public static string ProjectAllocationList(string organisationId, string projectId) =>
        $"ahp/{organisationId}/project/{projectId}/allocations";
}
