using System.Globalization;
using HE.Investment.AHP.Contract.Application;
using HE.Investments.AHP.IntegrationTests.Order03FillApplication.Data;

namespace HE.Investments.AHP.IntegrationTests.Order05ManageAllocation.Data.Allocation;

public class AllocationData
{
    public string AllocationId { get; private set; }

    public string AllocationName { get; private set; }

    public Tenure Tenure { get; private set; }

    public decimal TotalGrantAllocated { get; private set; }

    public decimal AmountPaid { get; private set; }

    public decimal AmountRemaining { get; private set; }

    public void GenerateAllocationName()
    {
        AllocationName = $"IT_allocation_{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}";
    }

    public void SetAllocationId(string allocationId)
    {
        AllocationId = allocationId;
    }

    public void SetFromApplicationData(ApplicationData applicationData, decimal totalGrantAllocated)
    {
        Tenure = applicationData.Tenure;
        TotalGrantAllocated = totalGrantAllocated;
    }
}
