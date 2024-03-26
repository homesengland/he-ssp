using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using HE.Investments.FrontDoor.Shared.Project.Contract;

namespace HE.Investments.FrontDoor.Shared.Project.Crm;

internal class InternalFrontDoorProjectEnumMapping : IFrontDoorProjectEnumMapping
{
    public IDictionary<SupportActivityType, int?> ActivityType => new Dictionary<SupportActivityType, int?>
    {
        { SupportActivityType.AcquiringLand, (int)invln_FrontDoorActivitiesinProject.Acquiringland },
        { SupportActivityType.DevelopingHomes, (int)invln_FrontDoorActivitiesinProject.Developinghomesincludinganyminorsiterelatedinfrastructure },
        { SupportActivityType.ProvidingInfrastructure, (int)invln_FrontDoorActivitiesinProject.Providinginfrastructure },
        { SupportActivityType.ManufacturingHomesWithinFactory, (int)invln_FrontDoorActivitiesinProject.Manufacturinghomeswithinafactory },
        { SupportActivityType.SellingLandToHomesEngland, (int)invln_FrontDoorActivitiesinProject.SellinglandtoHomesEngland },
        { SupportActivityType.Other, (int)invln_FrontDoorActivitiesinProject.Other },
    };

    public IDictionary<AffordableHomesAmount, int?> AffordableHomes => new Dictionary<AffordableHomesAmount, int?>
    {
        { AffordableHomesAmount.OnlyAffordableHomes, (int)invln_FrontDoorAmountofAffordableHomes.Iwanttodeliver100affordablehomes },
        { AffordableHomesAmount.OpenMarkedAndAffordableHomes, (int)invln_FrontDoorAmountofAffordableHomes.Iwanttodeilveropenmarkethomesandalsoaffordablehomesabovetheamountrequiredbyplanning },
        { AffordableHomesAmount.OpenMarkedAndRequiredAffordableHomes, (int)invln_FrontDoorAmountofAffordableHomes.Iwanttodeliveropenmarkethomesandtheamountofaffordablehomesrequiredbyplanning },
        { AffordableHomesAmount.OnlyOpenMarketHomes, (int)invln_FrontDoorAmountofAffordableHomes.Ionlywanttodeliveropenmarkethomes },
        { AffordableHomesAmount.Unknown, (int)invln_FrontDoorAmountofAffordableHomes.Idonotknow },
        { AffordableHomesAmount.Undefined, null },
    };

    public IDictionary<InfrastructureType, int?> Infrastructure => new Dictionary<InfrastructureType, int?>
    {
        { InfrastructureType.SitePreparation, (int)invln_FrontDoorInfrastructureDelivered.Sitepreparation },
        { InfrastructureType.Enabling, (int)invln_FrontDoorInfrastructureDelivered.Enabling },
        { InfrastructureType.PhysicalInfrastructure, (int)invln_FrontDoorInfrastructureDelivered.Physicalinfrastructure },
        { InfrastructureType.Other, (int)invln_FrontDoorInfrastructureDelivered.Other },
        { InfrastructureType.IDoNotKnow, (int)invln_FrontDoorInfrastructureDelivered.Idonotknow },
    };

    public IDictionary<ProjectGeographicFocus, int?> GeographicFocus => new Dictionary<ProjectGeographicFocus, int?>
    {
        { ProjectGeographicFocus.National, (int)invln_FrontDoorGeographicFocus.National },
        { ProjectGeographicFocus.Regional, (int)invln_FrontDoorGeographicFocus.Regional },
        { ProjectGeographicFocus.SpecificLocalAuthority, (int)invln_FrontDoorGeographicFocus.Specificlocalauthority },
        { ProjectGeographicFocus.Unknown, (int)invln_FrontDoorGeographicFocus.Idonotknow },
        { ProjectGeographicFocus.Undefined, null },
    };

    public IDictionary<RegionType, int?> RegionType => new Dictionary<RegionType, int?>
    {
        { Contract.RegionType.NorthEast, (int)invln_FrontDoorRegion.NorthEast },
        { Contract.RegionType.NorthWest, (int)invln_FrontDoorRegion.NorthWest },
        { Contract.RegionType.YorkshireAndTheHumber, (int)invln_FrontDoorRegion.YorkshireandtheHumber },
        { Contract.RegionType.EastMidlands, (int)invln_FrontDoorRegion.EastMidlands },
        { Contract.RegionType.WestMidlands, (int)invln_FrontDoorRegion.WestMidlands },
        { Contract.RegionType.EastOfEngland, (int)invln_FrontDoorRegion.EastofEngland },
        { Contract.RegionType.SouthEast, (int)invln_FrontDoorRegion.SouthEast },
        { Contract.RegionType.SouthWest, (int)invln_FrontDoorRegion.SouthWest },
        { Contract.RegionType.London, (int)invln_FrontDoorRegion.London },
    };

    public IDictionary<RequiredFundingOption, int?> FundingAmount => new Dictionary<RequiredFundingOption, int?>
    {
        { RequiredFundingOption.LessThan250K, (int)invln_FrontDoorAmountofFundingRequired.Lessthan250000 },
        { RequiredFundingOption.Between250KAnd1Mln, (int)invln_FrontDoorAmountofFundingRequired._250000to1million },
        { RequiredFundingOption.Between1MlnAnd5Mln, (int)invln_FrontDoorAmountofFundingRequired._1millionto5million },
        { RequiredFundingOption.Between5MlnAnd10Mln, (int)invln_FrontDoorAmountofFundingRequired._5millionto10million },
        { RequiredFundingOption.Between10MlnAnd30Mln, (int)invln_FrontDoorAmountofFundingRequired._10millionto30million },
        { RequiredFundingOption.Between30MlnAnd50Mln, (int)invln_FrontDoorAmountofFundingRequired._30millionto50million },
        { RequiredFundingOption.MoreThan50Mln, (int)invln_FrontDoorAmountofFundingRequired.Morethan50million },
        { RequiredFundingOption.IDoNotKnow, (int)invln_FrontDoorAmountofFundingRequired.Idonotknow },
    };

    public static IDictionary<SitePlanningStatus, int?> PlanningStatus => CommonEnumCrmMappings.PlanningStatus;
}
