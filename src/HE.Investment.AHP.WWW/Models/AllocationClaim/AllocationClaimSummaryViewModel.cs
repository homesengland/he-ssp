using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.Common.WWW.Models.Summary;

namespace HE.Investment.AHP.WWW.Models.AllocationClaim;

public class AllocationClaimSummaryViewModel : ISummaryViewModel
{
    public AllocationClaimSummaryViewModel(
        AllocationId allocationId,
        PhaseId phaseId,
        string allocationName,
        MilestoneType claimType,
        IList<SectionSummaryViewModel> sections,
        bool isEditable)
    {
        AllocationId = allocationId;
        PhaseId = phaseId;
        AllocationName = allocationName;
        ClaimType = claimType;
        Sections = sections;
        IsEditable = isEditable;
    }

    public AllocationId AllocationId { get; }

    public PhaseId PhaseId { get; }

    public string AllocationName { get; }

    public MilestoneType ClaimType { get; }

    public IList<SectionSummaryViewModel> Sections { get; }

    public bool IsEditable { get; }

    public bool IsReadOnly => !IsEditable;
}
