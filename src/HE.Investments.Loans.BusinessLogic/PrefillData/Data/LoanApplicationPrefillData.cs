using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.Loans.Contract.Application.Enums;

namespace HE.Investments.Loans.BusinessLogic.PrefillData.Data;

public record LoanApplicationPrefillData(
    FrontDoorProjectId ProjectId,
    FrontDoorSiteId? SiteId,
    string Name,
    FundingPurpose? FundingPurpose);
