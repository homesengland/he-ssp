using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.Common.WWW.Models.Summary;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.AllocationClaim.Factories;

public interface IAllocationClaimCheckAnswersViewModelFactory
{
    SectionSummaryViewModel CreateSummary(
        AllocationId allocationId,
        PhaseId phaseId,
        MilestoneClaim claim,
        IUrlHelper urlHelper,
        bool isEditable);
}
