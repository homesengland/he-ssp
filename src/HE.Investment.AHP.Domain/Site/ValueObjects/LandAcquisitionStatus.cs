using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Site.ValueObjects;

public class LandAcquisitionStatus : ValueObject, IQuestion
{
    public LandAcquisitionStatus(SiteLandAcquisitionStatus? landAcquisitionStatus = null)
    {
        Value = landAcquisitionStatus;
    }

    public SiteLandAcquisitionStatus? Value { get; }

    public bool HasFullLandOwnership => Value is SiteLandAcquisitionStatus.FullOwnership;

    public bool IsAnswered()
    {
        return Value is not null and not SiteLandAcquisitionStatus.Undefined;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
