using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.Common.Contract;
using HE.Investments.TestsUtils.Extensions;

namespace HE.Investments.AHP.IntegrationTests.Order03FillApplication.Data.DeliveryPhases;

public abstract class DeliveryPhaseDataBase<TDeliveryPhaseData> : INestedItemData
    where TDeliveryPhaseData : DeliveryPhaseDataBase<TDeliveryPhaseData>
{
    public string Id { get; private set; }

    public string Name { get; private set; }

    public abstract TypeOfHomes TypeOfHomes { get; }

    public abstract BuildActivityType BuildActivityType { get; }

    public DateDetails CompletionMilestoneDate { get; private set; }

    public DateDetails CompletionMilestonePaymentDate { get; private set; }

    public IList<HomeTypeDetails> DeliveryPhaseHomes { get; set; }

    protected abstract TDeliveryPhaseData DeliveryPhase { get; }

    public void SetDeliveryPhaseId(string deliveryPhaseId)
    {
        Id = deliveryPhaseId;
    }

    public TDeliveryPhaseData GenerateDeliveryPhase()
    {
        Name = $"IT-{TypeOfHomes}-{BuildActivityType}".WithTimestampSuffix();
        return DeliveryPhase;
    }

    public virtual TDeliveryPhaseData GenerateCompletionMilestone()
    {
        CompletionMilestoneDate = new DateDetails("01", "04", "2025");
        CompletionMilestonePaymentDate = new DateDetails("10", "04", "2025");
        return DeliveryPhase;
    }

    public abstract void PopulateAllData();
}
