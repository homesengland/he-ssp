using HE.InvestmentLoans.Common.Extensions;
using HE.Investments.Common.Domain;

namespace HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;

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
