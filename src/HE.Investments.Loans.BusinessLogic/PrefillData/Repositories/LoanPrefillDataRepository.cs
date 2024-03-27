using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.FrontDoor.Shared.Project.Repositories;
using HE.Investments.Loans.BusinessLogic.PrefillData.Crm;
using HE.Investments.Loans.BusinessLogic.PrefillData.Data;
using HE.Investments.Loans.BusinessLogic.Projects.Enums;
using HE.Investments.Loans.Contract.Application.Enums;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.PrefillData.Repositories;

public class LoanPrefillDataRepository : ILoanPrefillDataRepository
{
    private readonly IPrefillDataRepository _prefillDataRepository;

    private readonly ILoanPrefillDataCrmContext _crmContext;

    public LoanPrefillDataRepository(IPrefillDataRepository prefillDataRepository, ILoanPrefillDataCrmContext crmContext)
    {
        _prefillDataRepository = prefillDataRepository;
        _crmContext = crmContext;
    }

    public async Task<LoanApplicationPrefillData> GetLoanApplicationPrefillData(FrontDoorProjectId projectId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var prefillData = await _prefillDataRepository.GetProjectPrefillData(projectId, userAccount, cancellationToken);

        return new LoanApplicationPrefillData(
            projectId,
            prefillData.SiteId,
            prefillData.Name,
            MapFundingPurpose(prefillData.SupportActivities));
    }

    public async Task<LoanProjectPrefillData> GetLoanProjectPrefillData(LoanApplicationId applicationId, FrontDoorSiteId siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var frontDoorProjectId = await _crmContext.GetFrontDoorProjectId(applicationId, userAccount, cancellationToken);
        if (frontDoorProjectId.IsNotProvided())
        {
            throw new InvalidOperationException("Loan Application is not connected to Front Door project but Site is.");
        }

        var prefillData = await _prefillDataRepository.GetSitePrefillData(frontDoorProjectId!, siteId, cancellationToken);
        return new LoanProjectPrefillData(
            frontDoorProjectId!,
            siteId,
            prefillData.Name,
            prefillData.NumberOfHomes,
            MapPlanningPermissionStatus(prefillData.PlanningStatus),
            prefillData.LocalAuthorityName);
    }

    private static FundingPurpose? MapFundingPurpose(IReadOnlyCollection<SupportActivityType> supportActivities)
    {
        if (supportActivities.Count == 1 && supportActivities.Single() == SupportActivityType.DevelopingHomes)
        {
            return FundingPurpose.BuildingNewHomes;
        }

        return null;
    }

    private static PlanningPermissionStatus? MapPlanningPermissionStatus(SitePlanningStatus planningStatus)
    {
        return planningStatus switch
        {
            SitePlanningStatus.DetailedPlanningApprovalGranted => PlanningPermissionStatus.ReceivedFull,
            SitePlanningStatus.DetailedPlanningApprovalGrantedWithFurtherSteps => PlanningPermissionStatus.OutlineOrConsent,
            SitePlanningStatus.DetailedPlanningApplicationSubmitted => PlanningPermissionStatus.NotReceived,
            SitePlanningStatus.OutlinePlanningApprovalGranted => PlanningPermissionStatus.OutlineOrConsent,
            SitePlanningStatus.OutlinePlanningApplicationSubmitted => PlanningPermissionStatus.NotReceived,
            SitePlanningStatus.PlanningDiscussionsUnderwayWithThePlanningOffice => PlanningPermissionStatus.NotSubmitted,
            _ => null,
        };
    }
}
