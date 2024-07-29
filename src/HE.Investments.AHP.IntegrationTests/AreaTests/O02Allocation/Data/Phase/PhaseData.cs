using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Order03FillApplication.Data.DeliveryPhases;
using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Data.Phase;

public class PhaseData
{
    public string PhaseId { get; private set; }

    public string PhaseName { get; private set; }

    public int NumberOfHomes { get; private set; }

    public BuildActivityType BuildActivityType { get; private set; }

    public DateDetails AcquisitionForecastClaimDate { get; private set; }

    public DateDetails StartOnSiteForecastClaimDate { get; private set; }

    public DateDetails CompletionForecastClaimDate { get; private set; }

    public decimal AcquisitionAmountOfGrantApportioned { get; private set; }

    public string AcquisitionPercentageOfGrantApportioned => "40%";

    public decimal StartOnSiteAmountOfGrantApportioned { get; private set; }

    public string StartOnSitePercentageOfGrantApportioned => "35%";

    public decimal CompletionAmountOfGrantApportioned { get; private set; }

    public string CompletionPercentageOfGrantApportioned => "25%";

    public void SetPhaseId(string phaseId)
    {
        PhaseId = phaseId;
    }

    public void SetDataFromDeliveryPhase(RehabDeliveryPhase deliveryPhaseData, decimal requiredFunding, int numberOfHomes)
    {
        PhaseName = deliveryPhaseData.Name;
        NumberOfHomes = numberOfHomes;
        BuildActivityType = deliveryPhaseData.BuildActivityType;
        AcquisitionForecastClaimDate = deliveryPhaseData.AcquisitionMilestonePaymentDate;
        StartOnSiteForecastClaimDate = deliveryPhaseData.StartOnSiteMilestonePaymentDate;
        CompletionForecastClaimDate = deliveryPhaseData.CompletionMilestonePaymentDate;
        AcquisitionAmountOfGrantApportioned = requiredFunding / 2 * 0.4m;
        StartOnSiteAmountOfGrantApportioned = requiredFunding / 2 * 0.35m;
        CompletionAmountOfGrantApportioned = requiredFunding / 2 * 0.25m;
    }
}
