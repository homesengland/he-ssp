using HE.Investments.Common.CRM.Model;
using HE.Investments.Loans.Common.Utils.Enums;

namespace HE.Investments.Loans.BusinessLogic.Projects.Repositories.Mappers;

public static class ProjectCrmFieldNameMapper
{
    private static readonly string Name = nameof(invln_SiteDetails.invln_Name).ToLowerInvariant();
    private static readonly string AffordableHomes = nameof(invln_SiteDetails.invln_Affordablehousing).ToLowerInvariant();
    private static readonly string PlanningPermissionStatus = nameof(invln_SiteDetails.invln_planningpermissionstatus).ToLowerInvariant();
    private static readonly string HaveAPlanningReferenceNumber = nameof(invln_SiteDetails.invln_Haveaplanningreferencenumber).ToLowerInvariant();
    private static readonly string PlanningReferenceNumber = nameof(invln_SiteDetails.invln_Planningreferencenumber).ToLowerInvariant();
    private static readonly string SiteCoordinates = nameof(invln_SiteDetails.invln_Sitecoordinates).ToLowerInvariant();
    private static readonly string LandRegistryTitleNumber = nameof(invln_SiteDetails.invln_Landregistrytitlenumber).ToLowerInvariant();
    private static readonly string LocalAuthority = string.Join(",", nameof(invln_SiteDetails.invln_LocalAuthorityID), nameof(invln_SiteDetails.invln_HeLocalAuthorityId)).ToLowerInvariant();
    private static readonly string SiteOwnership = nameof(invln_SiteDetails.invln_Siteownership).ToLowerInvariant();
    private static readonly string NumberOfHomes = nameof(invln_SiteDetails.invln_Numberofhomes).ToLowerInvariant();
    private static readonly string TypeOfProject = nameof(invln_SiteDetails.invln_TypeofSite).ToLowerInvariant();
    private static readonly string TypeOfHomes = nameof(invln_SiteDetails.invln_Typeofhomes).ToLowerInvariant();
    private static readonly string TypeOfHomesOther = nameof(invln_SiteDetails.invln_OtherTypeofhomes).ToLowerInvariant();
    private static readonly string ProjectHasStartDate = nameof(invln_SiteDetails.invln_projecthasstartdate).ToLowerInvariant();
    private static readonly string StartDate = nameof(invln_SiteDetails.invln_startdate).ToLowerInvariant();
    private static readonly string DateOfPurchase = nameof(invln_SiteDetails.invln_Dateofpurchase).ToLowerInvariant();
    private static readonly string SiteCost = nameof(invln_SiteDetails.invln_Sitecost).ToLowerInvariant();
    private static readonly string CurrentValue = nameof(invln_SiteDetails.invln_currentvalue).ToLowerInvariant();
    private static readonly string ValuationSource = nameof(invln_SiteDetails.invln_Valuationsource).ToLowerInvariant();
    private static readonly string PublicSectorFunding = nameof(invln_SiteDetails.invln_Publicsectorfunding).ToLowerInvariant();
    private static readonly string GrantWhoProvided = nameof(invln_SiteDetails.invln_Whoprovided).ToLowerInvariant();
    private static readonly string GrantReason = nameof(invln_SiteDetails.invln_Reason).ToLowerInvariant();
    private static readonly string GrantHowMuch = nameof(invln_SiteDetails.invln_HowMuch).ToLowerInvariant();
    private static readonly string GrantOrFundName = nameof(invln_SiteDetails.invln_Nameofgrantfund).ToLowerInvariant();
    private static readonly string ChargesDebtExists = nameof(invln_SiteDetails.invln_Existinglegalcharges).ToLowerInvariant();
    private static readonly string ChargesDebtInformation = nameof(invln_SiteDetails.invln_Existinglegalchargesinformation).ToLowerInvariant();
    private static readonly string ProjectStatus = nameof(invln_SiteDetails.invln_completionstatus).ToLowerInvariant();
    private static readonly string FrontDoorSiteId = string.Join(",", nameof(invln_SiteDetails.invln_FDSiteId), nameof(invln_SiteDetails.invln_HeProjectLocalAuthorityId)).ToLowerInvariant();

    public static string Map(ProjectFieldsSet projectFieldsSet)
    {
        return projectFieldsSet switch
        {
            ProjectFieldsSet.StartDate => string.Join(",", ProjectHasStartDate, StartDate, FrontDoorSiteId),
            ProjectFieldsSet.ProjectName => string.Join(",", Name, FrontDoorSiteId),
            ProjectFieldsSet.AdditionalDetails => string.Join(",", DateOfPurchase, SiteCost, CurrentValue, ValuationSource, FrontDoorSiteId),
            ProjectFieldsSet.ChargesDebt => string.Join(",", ChargesDebtExists, ChargesDebtInformation, FrontDoorSiteId),
            ProjectFieldsSet.GrantFunding => string.Join(",", GrantWhoProvided, GrantReason, GrantHowMuch, GrantOrFundName, FrontDoorSiteId),
            ProjectFieldsSet.GrantFundingExists => string.Join(",", PublicSectorFunding, FrontDoorSiteId),
            ProjectFieldsSet.Location => string.Join(",", SiteCoordinates, LandRegistryTitleNumber, FrontDoorSiteId),
            ProjectFieldsSet.ManyHomes => string.Join(",", NumberOfHomes, FrontDoorSiteId),
            ProjectFieldsSet.TypeOfProject => string.Join(",", TypeOfProject, FrontDoorSiteId),
            ProjectFieldsSet.TypeOfHomes => string.Join(",", TypeOfHomes, TypeOfHomesOther, FrontDoorSiteId),
            ProjectFieldsSet.LocalAuthority => string.Join(",", LocalAuthority, FrontDoorSiteId),
            ProjectFieldsSet.Ownership => string.Join(",", SiteOwnership, LocalAuthority, FrontDoorSiteId),
            ProjectFieldsSet.PlanningPermissionStatus => string.Join(",", PlanningPermissionStatus, FrontDoorSiteId),
            ProjectFieldsSet.PlanningReferenceNumber => string.Join(",", PlanningReferenceNumber, FrontDoorSiteId, HaveAPlanningReferenceNumber),
            ProjectFieldsSet.PlanningReferenceNumberExists => string.Join(",", HaveAPlanningReferenceNumber, FrontDoorSiteId),
            ProjectFieldsSet.AffordableHomes => string.Join(",", AffordableHomes, FrontDoorSiteId),
            ProjectFieldsSet.GetStatus => string.Join(",", ProjectStatus, FrontDoorSiteId),
            ProjectFieldsSet.GetAllFields => string.Join(
                ",",
                Name,
                AffordableHomes,
                PlanningPermissionStatus,
                HaveAPlanningReferenceNumber,
                PlanningReferenceNumber,
                SiteCoordinates,
                LandRegistryTitleNumber,
                LocalAuthority,
                SiteOwnership,
                NumberOfHomes,
                TypeOfProject,
                TypeOfHomes,
                TypeOfHomesOther,
                ProjectHasStartDate,
                StartDate,
                DateOfPurchase,
                SiteCost,
                CurrentValue,
                ValuationSource,
                PublicSectorFunding,
                GrantWhoProvided,
                GrantReason,
                GrantHowMuch,
                GrantOrFundName,
                ChargesDebtExists,
                ChargesDebtInformation,
                ProjectStatus,
                FrontDoorSiteId),
            ProjectFieldsSet.SaveAllFields => string.Join(
                ",",
                Name,
                AffordableHomes,
                PlanningPermissionStatus,
                HaveAPlanningReferenceNumber,
                PlanningReferenceNumber,
                SiteCoordinates,
                LandRegistryTitleNumber,
                LocalAuthority,
                SiteOwnership,
                NumberOfHomes,
                TypeOfProject,
                TypeOfHomes,
                TypeOfHomesOther,
                ProjectHasStartDate,
                StartDate,
                DateOfPurchase,
                SiteCost,
                CurrentValue,
                ValuationSource,
                PublicSectorFunding,
                GrantWhoProvided,
                GrantReason,
                GrantHowMuch,
                GrantOrFundName,
                ChargesDebtExists,
                ChargesDebtInformation,
                ProjectStatus),
            _ => string.Join(",", ProjectStatus, FrontDoorSiteId),
        };
    }
}
