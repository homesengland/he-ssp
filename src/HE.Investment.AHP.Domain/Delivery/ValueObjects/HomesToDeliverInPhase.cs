using System.Globalization;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public sealed class HomesToDeliverInPhase : ValueObject
{
    private const string DisplayName = "number of homes being delivered";

    private const int MinValue = 0;

    private const int MaxValue = 999;

    public HomesToDeliverInPhase(HomeTypeId homeTypeId, int toDeliver)
    {
        if (toDeliver is < MinValue or > MaxValue)
        {
            OperationResult.New()
                .AddValidationError(AffectedField(homeTypeId), ValidationErrorMessage.MustBeNumber(DisplayName))
                .CheckErrors();
        }

        HomeTypeId = homeTypeId;
        ToDeliver = toDeliver;
    }

    public HomeTypeId HomeTypeId { get; }

    public int ToDeliver { get; }

    public static HomesToDeliverInPhase Create(HomeTypeId homeTypeId, string? toDeliver)
    {
        if (toDeliver.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(AffectedField(homeTypeId), ValidationErrorMessage.MissingRequiredField(DisplayName))
                .CheckErrors();
        }

        if (!int.TryParse(toDeliver, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue))
        {
            OperationResult.New()
                .AddValidationError(AffectedField(homeTypeId), ValidationErrorMessage.MustBeNumber(DisplayName))
                .CheckErrors();
        }

        return new HomesToDeliverInPhase(homeTypeId, parsedValue);
    }

    public static string AffectedField(HomeTypeId homeTypeId) => $"HomesToDeliver[{homeTypeId}]";

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return HomeTypeId;
        yield return ToDeliver;
    }
}
