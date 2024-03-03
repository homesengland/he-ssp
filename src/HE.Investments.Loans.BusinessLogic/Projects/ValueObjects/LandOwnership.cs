using HE.Investments.Common.Domain;
using HE.Investments.Common.WWW.Extensions;

namespace HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;

public class LandOwnership : ValueObject
{
    public LandOwnership(bool applicationHasFullOwnership)
    {
        ApplicantHasFullOwnership = applicationHasFullOwnership;
    }

    public bool ApplicantHasFullOwnership { get; }

    public static LandOwnership From(string ownershipAnswer) => new(ownershipAnswer.MapToNonNullableBool());

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return ApplicantHasFullOwnership;
    }
}
