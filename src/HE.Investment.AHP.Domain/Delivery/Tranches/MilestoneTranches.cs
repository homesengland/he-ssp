using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Delivery.Tranches;

public class MilestoneTranches : ValueObject, IQuestion
{
    private const string UiFieldName = "Value";

    private MilestoneTranches(decimal? acquisition, decimal? startOnSite, decimal? completion, decimal? grantApportioned)
    {
        GrantApportioned = Validate(grantApportioned, "Grant apportioned");
        MinimalCompletionTranche = GrantApportioned.GetValueOrDefault() * 0.05m;
        MaxTranche = GrantApportioned.GetValueOrDefault() - MinimalCompletionTranche;
        Acquisition = Validate(acquisition, "Acquisition tranche");
        StartOnSite = Validate(startOnSite, "Start on site tranche");
        Completion = Validate(completion, "Completion tranche");
    }

    public static MilestoneTranches NotProvided => new(null, null, null, null);

    public decimal? Acquisition { get; }

    public decimal? StartOnSite { get; }

    public decimal? Completion { get; }

    public decimal? GrantApportioned { get; }

    public bool IsAmendRequested => Acquisition.IsProvided() || StartOnSite.IsProvided() || Completion.IsProvided();

    public decimal MinimalCompletionTranche { get; }

    public decimal MaxTranche { get; }

    public MilestoneTranches WithGrantApportioned(decimal? grantApportioned)
    {
        return new MilestoneTranches(Acquisition, StartOnSite, Completion, grantApportioned);
    }

    public MilestoneTranches WithAcquisition(decimal? acquisition)
    {
        if (acquisition > MaxTranche)
        {
            OperationResult.ThrowValidationError(UiFieldName, "Acquisition tranche must be at max 95% or less of the grant apportioned");
        }

        return new MilestoneTranches(acquisition, StartOnSite, Completion, GrantApportioned);
    }

    public MilestoneTranches WithCompletion(decimal? completion)
    {
        if (completion < MinimalCompletionTranche)
        {
            OperationResult.ThrowValidationError(UiFieldName, "Completion tranche must be at least 5% or more of the grant apportioned");
        }

        return new MilestoneTranches(Acquisition, StartOnSite, completion, GrantApportioned);
    }

    public MilestoneTranches WithStartOnSite(decimal? startOnSite)
    {
        if (startOnSite > MaxTranche)
        {
            OperationResult.ThrowValidationError(UiFieldName, "Start on Site tranche must be at max 95% or less of the grant apportioned");
        }

        return new MilestoneTranches(Acquisition, startOnSite, Completion, GrantApportioned);
    }

    public bool IsAnswered() => Acquisition.IsProvided() && StartOnSite.IsProvided() && Completion.IsProvided();

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Acquisition;
        yield return StartOnSite;
        yield return Completion;
    }

    private static decimal? Validate(decimal? value, string displayName)
    {
        return value is null ? null : PoundsPencesValidator.Validate(value.Value, UiFieldName, displayName);
    }
}
