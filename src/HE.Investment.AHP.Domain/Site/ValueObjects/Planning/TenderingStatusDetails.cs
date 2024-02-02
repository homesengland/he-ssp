using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning;

public class TenderingStatusDetails : ValueObject, IQuestion
{
    public TenderingStatusDetails(SiteTenderingStatus? tenderingStatus = null)
    {
        TenderingStatus = tenderingStatus;
    }

    public SiteTenderingStatus? TenderingStatus { get; }

    public bool IsAnswered()
    {
        return TenderingStatus.IsProvided();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return TenderingStatus;
    }
}
