using HE.Investments.Common.CRM.Model;
using HE.Investments.Loans.Common.Utils.Enums;

namespace HE.Investments.Loans.BusinessLogic.Projects.Repositories.Mappers;

public static class ProjectCrmFieldNameMapper
{
    private static readonly string Name = $"{nameof(invln_SiteDetails.invln_Name).ToLowerInvariant()}";
    private static readonly string AffordableHomes = $"{nameof(invln_SiteDetails.invln_Affordablehousing).ToLowerInvariant()}";
    private static readonly string PlanningPermissionStatus = $"{nameof(invln_SiteDetails.invln_planningpermissionstatus).ToLowerInvariant()}";
    private static readonly string HaveAPlanningReferenceNumber = $"{nameof(invln_SiteDetails.invln_Haveaplanningreferencenumber).ToLowerInvariant()}";
    private static readonly string PlanningReferenceNumber = $"{nameof(invln_SiteDetails.invln_Planningreferencenumber).ToLowerInvariant()}";
    private static readonly string SiteCoordinates = $"{nameof(invln_SiteDetails.invln_Sitecoordinates).ToLowerInvariant()}";
    private static readonly string LandRegistryTitleNumber = $"{nameof(invln_SiteDetails.invln_Landregistrytitlenumber).ToLowerInvariant()}";
    private static readonly string LocalAuthority = $"{nameof(invln_SiteDetails.invln_LocalAuthorityID).ToLowerInvariant()}";
    private static readonly string SiteOwnership = $"{nameof(invln_SiteDetails.invln_Siteownership).ToLowerInvariant()}";
    private static readonly string NumberOfHomes = $"{nameof(invln_SiteDetails.invln_Numberofhomes).ToLowerInvariant()}";
    private static readonly string TypeOfProject = $"{nameof(invln_SiteDetails.invln_TypeofSite).ToLowerInvariant()}";
    private static readonly string TypeOfHomes = $"{nameof(invln_SiteDetails.invln_Typeofhomes).ToLowerInvariant()}";
    private static readonly string TypeOfHomesOther = $"{nameof(invln_SiteDetails.invln_OtherTypeofhomes).ToLowerInvariant()}";
    private static readonly string ProjectHasStartDate = $"{nameof(invln_SiteDetails.invln_projecthasstartdate).ToLowerInvariant()}";
    private static readonly string StartDate = $"{nameof(invln_SiteDetails.invln_startdate).ToLowerInvariant()}";
    private static readonly string DateOfPurchase = $"{nameof(invln_SiteDetails.invln_Dateofpurchase).ToLowerInvariant()}";
    private static readonly string SiteCost = $"{nameof(invln_SiteDetails.invln_Sitecost).ToLowerInvariant()}";
    private static readonly string CurrentValue = $"{nameof(invln_SiteDetails.invln_currentvalue).ToLowerInvariant()}";
    private static readonly string ValuationSource = $"{nameof(invln_SiteDetails.invln_Valuationsource).ToLowerInvariant()}";
    private static readonly string PublicSectorFunding = $"{nameof(invln_SiteDetails.invln_Publicsectorfunding).ToLowerInvariant()}";
    private static readonly string GrantWhoProvided = $"{nameof(invln_SiteDetails.invln_Whoprovided).ToLowerInvariant()}";
    private static readonly string GrantReason = $"{nameof(invln_SiteDetails.invln_Reason).ToLowerInvariant()}";
    private static readonly string GrantHowMuch = $"{nameof(invln_SiteDetails.invln_HowMuch).ToLowerInvariant()}";
    private static readonly string GrantOrFundName = $"{nameof(invln_SiteDetails.invln_Nameofgrantfund).ToLowerInvariant()}";
    private static readonly string ChargesDebtExists = $"{nameof(invln_SiteDetails.invln_Existinglegalcharges).ToLowerInvariant()}";
    private static readonly string ChargesDebtInformation = $"{nameof(invln_SiteDetails.invln_Existinglegalchargesinformation).ToLowerInvariant()}";
    private static readonly string ProjectStatus = $"{nameof(invln_SiteDetails.invln_completionstatus).ToLowerInvariant()}";
    private static readonly string FrontDoorSiteId = string.Empty; // TODO: AB#91906 use "invln_fdprojectsiteid" when CRM provides property

    public static string Map(ProjectFieldsSet projectFieldsSet)
    {
        var result = projectFieldsSet switch
        {
            ProjectFieldsSet.StartDate => string.Join(",", ProjectHasStartDate, StartDate),
            ProjectFieldsSet.ProjectName => Name,
            ProjectFieldsSet.AdditionalDetails => string.Join(",", DateOfPurchase, SiteCost, CurrentValue, ValuationSource),
            ProjectFieldsSet.ChargesDebt => string.Join(",", ChargesDebtExists, ChargesDebtInformation),
            ProjectFieldsSet.GrantFunding => string.Join(",", GrantWhoProvided, GrantReason, GrantHowMuch, GrantOrFundName),
            ProjectFieldsSet.GrantFundingExists => PublicSectorFunding,
            ProjectFieldsSet.Location => string.Join(",", SiteCoordinates, LandRegistryTitleNumber),
            ProjectFieldsSet.ManyHomes => NumberOfHomes,
            ProjectFieldsSet.TypeOfProject => TypeOfProject,
            ProjectFieldsSet.TypeOfHomes => string.Join(",", TypeOfHomes, TypeOfHomesOther),
            ProjectFieldsSet.LocalAuthority => LocalAuthority,
            ProjectFieldsSet.Ownership => string.Join(",", SiteOwnership, LocalAuthority),
            ProjectFieldsSet.PlanningPermissionStatus => PlanningPermissionStatus,
            ProjectFieldsSet.PlanningReferenceNumber => PlanningReferenceNumber,
            ProjectFieldsSet.PlanningReferenceNumberExists => HaveAPlanningReferenceNumber,
            ProjectFieldsSet.AffordableHomes => AffordableHomes,
            ProjectFieldsSet.GetStatus => ProjectStatus,
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
                ProjectStatus),
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
            _ => ProjectStatus,
        };

        // TODO: AB#91906 remove this condition after integration with CRM because FrontDoorSiteId name should not be empty
        if (string.IsNullOrWhiteSpace(FrontDoorSiteId))
        {
            return result;
        }

        return $"{result},{FrontDoorSiteId}";
    }
}
