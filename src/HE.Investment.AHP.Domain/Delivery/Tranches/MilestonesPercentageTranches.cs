using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Delivery.Tranches;

public class MilestonesPercentageTranches : ValueObject
{
    private const string UiFieldName = "Value";

    private const decimal MinimalCompletionTranche = 0.05m;

    private const decimal MaxTranche = 0.95m;

    public MilestonesPercentageTranches(WholePercentage? acquisitionPercentage, WholePercentage? startOnSitePercentage, WholePercentage? completionPercentage)
    {
        Acquisition = acquisitionPercentage;
        StartOnSite = startOnSitePercentage;
        Completion = completionPercentage;
    }

    public static MilestonesPercentageTranches LackOfCalculation => new(null, null, null);

    public static MilestonesPercentageTranches NotProvided => new(null, null, null);

    public static MilestonesPercentageTranches OnlyCompletion => new(null, null, WholePercentage.Hundered);

    public WholePercentage? Acquisition { get; }

    public WholePercentage? StartOnSite { get; }

    public WholePercentage? Completion { get; }

    public MilestonesPercentageTranches WithAcquisition(WholePercentage acquisition)
    {
        if (acquisition.Value > MaxTranche)
        {
            OperationResult.ThrowValidationError(UiFieldName, "Acquisition tranche must be at max 95% or less of the grant apportioned");
        }

        return new MilestonesPercentageTranches(acquisition, StartOnSite, Completion);
    }

    public MilestonesPercentageTranches WithCompletion(WholePercentage completion)
    {
        if (completion.Value < MinimalCompletionTranche)
        {
            OperationResult.ThrowValidationError(UiFieldName, "Completion tranche must be at least 5% or more of the grant apportioned");
        }

        return new MilestonesPercentageTranches(Acquisition, StartOnSite, completion);
    }

    public MilestonesPercentageTranches WithStartOnSite(WholePercentage startOnSite)
    {
        if (startOnSite.Value > MaxTranche)
        {
            OperationResult.ThrowValidationError(UiFieldName, "Start on Site tranche must be at max 95% or less of the grant apportioned");
        }

        return new MilestonesPercentageTranches(Acquisition, startOnSite, Completion);
    }

    public bool IsSumUpTo100Percentage() => (Acquisition?.Value ?? 0m) + (StartOnSite?.Value ?? 0m) + (Completion?.Value ?? 0m) == 1m;

    public bool IsAnyPercentageProvided() => Acquisition.IsProvided() || StartOnSite.IsProvided() || Completion.IsProvided();

    public bool AreAllPercentagesProvided() => Acquisition.IsProvided() && StartOnSite.IsProvided() && Completion.IsProvided();

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return StartOnSite;
        yield return Acquisition;
        yield return Completion;
    }
}
