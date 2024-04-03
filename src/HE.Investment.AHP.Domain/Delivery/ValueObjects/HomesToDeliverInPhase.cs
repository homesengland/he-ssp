using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public sealed class HomesToDeliverInPhase : TheRequiredIntValueObject
{
    private const string DisplayName = "number of homes being delivered";

    private const int MinValue = 0;

    private const int MaxValue = 999;

    public HomesToDeliverInPhase(HomeTypeId homeTypeId, int toDeliver)
        : base(toDeliver, AffectedField(homeTypeId), DisplayName, MinValue, MaxValue)
    {
        HomeTypeId = homeTypeId;
    }

    public HomesToDeliverInPhase(HomeTypeId homeTypeId, string? toDeliver)
        : base(toDeliver, AffectedField(homeTypeId), DisplayName, MinValue, MaxValue)
    {
        HomeTypeId = homeTypeId;
    }

    public HomeTypeId HomeTypeId { get; }

    public static string AffectedField(HomeTypeId homeTypeId) => $"HomesToDeliver[{homeTypeId}]";

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return HomeTypeId;
        yield return Value;
    }
}
