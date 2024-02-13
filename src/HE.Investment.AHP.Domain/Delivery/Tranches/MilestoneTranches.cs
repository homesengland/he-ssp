using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Delivery.Tranches;

public class MilestoneTranches : ValueObject
{
    private const string UiFieldName = "Value";

    private const decimal MinimalCompletionTranche = 0.05m;

    private const decimal MaxTranche = 0.95m;

    private MilestoneTranches(WholePercentage? acquisition, WholePercentage? startOnSite, WholePercentage? completion)
    {
        Acquisition = acquisition;
        StartOnSite = startOnSite;
        Completion = completion;
    }

    public static MilestoneTranches NotProvided => new(null, null, null);

    public WholePercentage? Acquisition { get; }

    public WholePercentage? StartOnSite { get; }

    public WholePercentage? Completion { get; }

    public bool IsAmendRequested => Acquisition.IsProvided() || StartOnSite.IsProvided() || Completion.IsProvided();

    public bool IsSumUpTo => (Acquisition?.Value ?? 0m + StartOnSite?.Value ?? 0m + Completion?.Value ?? 0m) == 1m;

    public MilestoneTranches WithAcquisition(WholePercentage acquisition)
    {
        if (acquisition.Value > MaxTranche)
        {
            OperationResult.ThrowValidationError(UiFieldName, "Acquisition tranche must be at max 95% or less of the grant apportioned");
        }

        return new MilestoneTranches(acquisition, StartOnSite, Completion);
    }

    public MilestoneTranches WithCompletion(WholePercentage completion)
    {
        if (completion.Value < MinimalCompletionTranche)
        {
            OperationResult.ThrowValidationError(UiFieldName, "Completion tranche must be at least 5% or more of the grant apportioned");
        }

        return new MilestoneTranches(Acquisition, StartOnSite, completion);
    }

    public MilestoneTranches WithStartOnSite(WholePercentage startOnSite)
    {
        if (startOnSite.Value > MaxTranche)
        {
            OperationResult.ThrowValidationError(UiFieldName, "Start on Site tranche must be at max 95% or less of the grant apportioned");
        }

        return new MilestoneTranches(Acquisition, startOnSite, Completion);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Acquisition;
        yield return StartOnSite;
        yield return Completion;
    }
}
