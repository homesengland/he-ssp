using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.Tranches;

public class MilestonesPercentageTranches : ValueObject
{
    private const string UiFieldName = "Value";

    private const decimal MinimalCompletionTranche = 0.05m;

    private const decimal MaxTranche = 0.95m;

    private const decimal MaxCompletionTranche = 1m;

    public MilestonesPercentageTranches(WholePercentage? acquisitionPercentage, WholePercentage? startOnSitePercentage, WholePercentage? completionPercentage)
    {
        Acquisition = acquisitionPercentage;
        StartOnSite = startOnSitePercentage;
        Completion = completionPercentage;
    }

    public static MilestonesPercentageTranches NotProvided => new(null, null, null);

    public WholePercentage? Acquisition { get; }

    public WholePercentage? StartOnSite { get; }

    public WholePercentage? Completion { get; }

    public MilestonesPercentageTranches WithAcquisition(WholePercentage acquisition)
    {
        if (acquisition.Value > MaxTranche)
        {
            OperationResult.ThrowValidationError(UiFieldName, "Acquisition tranche must be 95% or less of the grant apportioned");
        }

        return new MilestonesPercentageTranches(acquisition, StartOnSite, Completion);
    }

    public MilestonesPercentageTranches WithCompletion(WholePercentage completion)
    {
        if (completion.Value is < MinimalCompletionTranche or > MaxCompletionTranche)
        {
            OperationResult.ThrowValidationError(UiFieldName, "Completion tranche must be between 5% and 100% of the grant apportioned");
        }

        return new MilestonesPercentageTranches(Acquisition, StartOnSite, completion);
    }

    public MilestonesPercentageTranches WithStartOnSite(WholePercentage startOnSite)
    {
        if (startOnSite.Value > MaxTranche)
        {
            OperationResult.ThrowValidationError(UiFieldName, "Start on site tranche must be 95% or less of the grant apportioned");
        }

        return new MilestonesPercentageTranches(Acquisition, startOnSite, Completion);
    }

    public bool IsSumUpTo100Percentage() => (Acquisition?.Value ?? 0m) + (StartOnSite?.Value ?? 0m) + (Completion?.Value ?? 0m) == 1m;

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return StartOnSite;
        yield return Acquisition;
        yield return Completion;
    }
}
