using System.Globalization;
using HE.Investment.AHP.Contract.Application;
using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Order03FillApplication.Data;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Data.Allocation;

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

    public void SetFromApplicationData(ApplicationData applicationData, decimal totalGrantAllocated, decimal grantAmountPaid = 0)
    {
        Tenure = applicationData.Tenure;
        CalculateGrantDetails(totalGrantAllocated, grantAmountPaid);
        SetGrantDetailsAsZeroAndRemoveItWhen();
    }

    private void SetGrantDetailsAsZeroAndRemoveItWhen()
    {
        // TODO: 104185 - Remove when CRM implementation is done
        TotalGrantAllocated = 0;
        AmountPaid = 0;
        AmountRemaining = 0;
    }

    private void CalculateGrantDetails(decimal totalGrantAllocated, decimal grantAmountPaid = 0)
    {
        TotalGrantAllocated = totalGrantAllocated;
        AmountPaid = grantAmountPaid;
        AmountRemaining = TotalGrantAllocated - grantAmountPaid;
    }
}
