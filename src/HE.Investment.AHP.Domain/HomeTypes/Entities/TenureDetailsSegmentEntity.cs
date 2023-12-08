using System.Globalization;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[HomeTypeSegmentType(HomeTypeSegmentType.TenureDetails)]
public class TenureDetailsSegmentEntity : IHomeTypeSegmentEntity
{
    private readonly ModificationTracker _modificationTracker;

    public TenureDetailsSegmentEntity(
        HomeMarketValue? homeMarketValue = null,
        HomeWeeklyRent? homeWeeklyRent = null,
        AffordableWeeklyRent? affordableWeeklyRent = null,
        AffordableRentAsPercentageOfMarketRent? affordableRentAsPercentageOfMarketRent = null,
        YesNoType targetRentExceedMarketRent = YesNoType.Undefined,
        YesNoType exemptFromTheRightToSharedOwnership = YesNoType.Undefined,
        MoreInformation? exemptionJustification = null)
    {
        _modificationTracker = new ModificationTracker(() => SegmentModified?.Invoke());
        HomeMarketValue = homeMarketValue;
        HomeWeeklyRent = homeWeeklyRent;
        AffordableWeeklyRent = affordableWeeklyRent;
        AffordableRentAsPercentageOfMarketRent = affordableRentAsPercentageOfMarketRent;
        TargetRentExceedMarketRent = new TargetRentExceedMarketRent(targetRentExceedMarketRent);
        ExemptFromTheRightToSharedOwnership = exemptFromTheRightToSharedOwnership;
        ExemptionJustification = exemptionJustification;
    }

    public event EntityModifiedEventHandler? SegmentModified;

    public bool IsModified => _modificationTracker.IsModified;

    public HomeMarketValue? HomeMarketValue { get; private set; }

    public HomeWeeklyRent? HomeWeeklyRent { get; private set; }

    public AffordableWeeklyRent? AffordableWeeklyRent { get; private set; }

    public AffordableRentAsPercentageOfMarketRent? AffordableRentAsPercentageOfMarketRent { get; private set; }

    public TargetRentExceedMarketRent? TargetRentExceedMarketRent { get; private set; }

    public YesNoType ExemptFromTheRightToSharedOwnership { get; private set; }

    public MoreInformation? ExemptionJustification { get; private set; }

    public static decimal CalculateAffordableRent(string? homeWeeklyRent, string? affordableWeeklyRent)
    {
        var result = 00.00m;

        if (decimal.TryParse(homeWeeklyRent!, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedHomeWeeklyRent)
            && decimal.TryParse(affordableWeeklyRent!, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedAffordableWeeklyRent)
            && decimal.Round(parsedHomeWeeklyRent, 2) == parsedHomeWeeklyRent
            && decimal.Round(parsedAffordableWeeklyRent, 2) == parsedAffordableWeeklyRent)
        {
            result = parsedAffordableWeeklyRent / parsedHomeWeeklyRent * 100;
            result = Math.Round(result, 2);
        }

        return result;
    }

    public void ChangeHomeMarketValue(string? homeMarketValue, bool isCalculation = false)
    {
        var newValue = homeMarketValue.IsProvided() || isCalculation
            ? new HomeMarketValue(homeMarketValue, isCalculation)
            : null;

        HomeMarketValue = _modificationTracker.Change(HomeMarketValue, newValue);
    }

    public void ChangeHomeWeeklyRent(string? homeWeeklyRent, bool isCalculation = false)
    {
        var newValue = homeWeeklyRent.IsProvided() || isCalculation
            ? new HomeWeeklyRent(homeWeeklyRent, isCalculation)
            : null;

        HomeWeeklyRent = _modificationTracker.Change(HomeWeeklyRent, newValue);
    }

    public void ChangeAffordableWeeklyRent(string? affordableWeeklyRent, bool isCalculation = false)
    {
        var newValue = affordableWeeklyRent.IsProvided() || isCalculation
            ? new AffordableWeeklyRent(affordableWeeklyRent, isCalculation)
            : null;

        AffordableWeeklyRent = _modificationTracker.Change(AffordableWeeklyRent, newValue);
    }

    public void CalculateAffordableRentAsPercentageOfMarketRent(string? homeWeeklyRent, string? affordableWeeklyRent)
    {
        var result = CalculateAffordableRent(homeWeeklyRent, affordableWeeklyRent);

        var newValue = new AffordableRentAsPercentageOfMarketRent(result);
        AffordableRentAsPercentageOfMarketRent = _modificationTracker.Change(AffordableRentAsPercentageOfMarketRent, newValue);

        if (result > 80 && TargetRentExceedMarketRent?.Value == YesNoType.No)
        {
            OperationResult.New()
                .AddValidationError(nameof(AffordableRentAsPercentageOfMarketRent), ValidationErrorMessage.ProspectiveRentExceed80Percent())
                .CheckErrors();
        }
    }

    public void ChangeTargetRentExceedMarketRent(YesNoType targetRentExceedMarketRent, bool isCalculation = false)
    {
        var newValue = targetRentExceedMarketRent.IsProvided() || isCalculation
            ? new TargetRentExceedMarketRent(targetRentExceedMarketRent, isCalculation)
            : null;

        TargetRentExceedMarketRent = _modificationTracker.Change(TargetRentExceedMarketRent, newValue);
    }

    public void ChangeExemptFromTheRightToSharedOwnership(YesNoType exemptFromTheRightToSharedOwnership)
    {
        ExemptFromTheRightToSharedOwnership = _modificationTracker.Change(ExemptFromTheRightToSharedOwnership, exemptFromTheRightToSharedOwnership);
    }

    public void ChangeExemptionJustification(MoreInformation? newValue)
    {
        ExemptionJustification = _modificationTracker.Change(ExemptionJustification, newValue);
    }

    public IHomeTypeSegmentEntity Duplicate()
    {
        return new TenureDetailsSegmentEntity(
            HomeMarketValue,
            HomeWeeklyRent,
            AffordableWeeklyRent,
            AffordableRentAsPercentageOfMarketRent,
            TargetRentExceedMarketRent.IsNotProvided() ? YesNoType.Undefined : TargetRentExceedMarketRent!.Value,
            ExemptFromTheRightToSharedOwnership,
            ExemptionJustification);
    }

    public bool IsRequired(HousingType housingType)
    {
        return true;
    }

    public bool IsCompleted()
    {
        return HomeMarketValue.IsProvided()
               && HomeWeeklyRent.IsProvided()
               && AffordableWeeklyRent.IsProvided()
               && AffordableRentAsPercentageOfMarketRent.IsProvided()
               && TargetRentExceedMarketRent?.Value != YesNoType.Undefined
               && ExemptFromTheRightToSharedOwnership != YesNoType.Undefined // todo to be change because there will be different workflows
               && ExemptionJustification.IsProvided();  // todo to be change because there will be different workflows
    }

    public void HousingTypeChanged(HousingType sourceHousingType, HousingType targetHousingType)
    {
        if (targetHousingType is HousingType.Undefined or HousingType.General)
        {
            ChangeHomeMarketValue(null);
            ChangeHomeWeeklyRent(null);
            ChangeAffordableWeeklyRent(null);
            CalculateAffordableRentAsPercentageOfMarketRent(HomeWeeklyRent?.ToString(), AffordableWeeklyRent?.ToString());
            ChangeTargetRentExceedMarketRent(YesNoType.Undefined);
            ChangeExemptFromTheRightToSharedOwnership(YesNoType.Undefined);
            ChangeExemptionJustification(null);
        }
    }
}
