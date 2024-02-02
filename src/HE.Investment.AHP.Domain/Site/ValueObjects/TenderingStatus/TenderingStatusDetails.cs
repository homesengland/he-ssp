using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.TenderingStatus;

public class TenderingStatusDetails : ValueObject, IQuestion
{
    public TenderingStatusDetails(
        SiteTenderingStatus? tenderingStatus = null,
        ContractorName? contractorName = null,
        bool? isSmeContractor = null)
    {
        TenderingStatus = tenderingStatus;
        ContractorName = contractorName;
        IsSmeContractor = isSmeContractor;
    }

    public SiteTenderingStatus? TenderingStatus { get; }

    public ContractorName? ContractorName { get; }

    public bool? IsSmeContractor { get; }

    public bool IsAnswered()
    {
        return TenderingStatus switch
        {
            SiteTenderingStatus.NotApplicable => true,
            SiteTenderingStatus.ConditionalWorksContract => ContractorName.IsProvided() && IsSmeContractor.IsProvided(),
            SiteTenderingStatus.UnconditionalWorksContract => ContractorName.IsProvided() && IsSmeContractor.IsProvided(),
            _ => false,
        };
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return TenderingStatus;
        yield return ContractorName;
        yield return IsSmeContractor;
    }
}
