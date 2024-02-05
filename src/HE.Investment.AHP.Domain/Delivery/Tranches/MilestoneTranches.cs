using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Delivery.Tranches;

public class MilestoneTranches : ValueObject, IQuestion
{
    public MilestoneTranches(decimal? acquisition, decimal? startOnSite, decimal? completion)
    {
        Acquisition = Validate(acquisition, "Acquisition tranche");
        StartOnSite = Validate(startOnSite, "Start on site tranche");
        Completion = Validate(completion, "Completion tranche");
    }

    public static MilestoneTranches NotProvided => new(null, null, null);

    public decimal? Acquisition { get; }

    public decimal? StartOnSite { get; }

    public decimal? Completion { get; }

    public bool IsAmended => Acquisition.IsProvided() || StartOnSite.IsProvided() || Completion.IsProvided();

    public MilestoneTranches WithAcquisition(decimal? acquisition)
    {
        return new MilestoneTranches(acquisition, StartOnSite, Completion);
    }

    public MilestoneTranches WithCompletion(decimal? completion)
    {
        return new MilestoneTranches(Acquisition, StartOnSite, completion);
    }

    public MilestoneTranches WithStartOnSite(decimal? startOnSite)
    {
        return new MilestoneTranches(Acquisition, startOnSite, Completion);
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
        return value is null ? null : PoundsPencesValidator.Validate(value.Value, "Value", displayName);
    }
}
