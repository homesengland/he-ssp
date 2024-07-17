namespace HE.Investments.AHP.IntegrationTests.Order03FillApplication.Data.Allocation;

public class AllocationData
{
    public string AllocationId { get; private set; }

    public string AllocationName { get; private set; }

    public void SetAllocationName(string allocationName)
    {
        AllocationName = allocationName;
    }

    public void SetAllocationId(string allocationId)
    {
        AllocationId = allocationId;
    }
}
