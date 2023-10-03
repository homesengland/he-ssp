using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.Common.Domain;
using HE.InvestmentLoans.Common.Extensions;

namespace HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;

public class LandOwnership : ValueObject
{
    public LandOwnership(bool applicationHasFullOwnership)
    {
        ApplicantHasFullOwnership = applicationHasFullOwnership;
    }

    public bool ApplicantHasFullOwnership { get; private set; }

    public static LandOwnership From(string ownershipAnswer) => new(ownershipAnswer.MapToNonNullableBool());

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return ApplicantHasFullOwnership;
    }
}
