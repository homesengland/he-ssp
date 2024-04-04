using HE.Investments.Common.CRM.Model;
using HE.Investments.FrontDoor.Shared.Project.Contract;

namespace HE.Investments.FrontDoor.Shared.Project.Crm;

internal class ExternalFrontDoorProjectEnumMapping : IFrontDoorProjectEnumMapping
{
    public IDictionary<SupportActivityType, int?> ActivityType => new Dictionary<SupportActivityType, int?>
    {
        { SupportActivityType.AcquiringLand, (int)he_pipeline_he_ActivitiesinThisProject.Acquiringland },
        { SupportActivityType.DevelopingHomes, (int)he_pipeline_he_ActivitiesinThisProject.Developinghomesincludinganyminorsiterelatedinfrastructure },
        { SupportActivityType.ProvidingInfrastructure, (int)he_pipeline_he_ActivitiesinThisProject.Providinginfrastructure },
        { SupportActivityType.ManufacturingHomesWithinFactory, (int)he_pipeline_he_ActivitiesinThisProject.Manufacturinghomeswithinafactory },
        { SupportActivityType.SellingLandToHomesEngland, (int)he_pipeline_he_ActivitiesinThisProject.SellinglandtoHomesEngland },
        { SupportActivityType.Other, (int)he_pipeline_he_ActivitiesinThisProject.Other },
    };

    public IDictionary<AffordableHomesAmount, int?> AffordableHomes => new Dictionary<AffordableHomesAmount, int?>
    {
        { AffordableHomesAmount.OnlyAffordableHomes, (int)he_Pipeline_he_amountofaffordablehomes.Iwanttodeliver100affordablehomes },
        { AffordableHomesAmount.OpenMarkedAndAffordableHomes, (int)he_Pipeline_he_amountofaffordablehomes.Iwanttodeliveropenmarkethomesandalsoaffordablehomesabovetheamountrequiredbyplanning },
        { AffordableHomesAmount.OpenMarkedAndRequiredAffordableHomes, (int)he_Pipeline_he_amountofaffordablehomes.Iwanttodeliveropenmarkethomesandtheamountofaffordablehomesrequiredbyplanning },
        { AffordableHomesAmount.OnlyOpenMarketHomes, (int)he_Pipeline_he_amountofaffordablehomes.Ionlywanttodeliveropenmarkethomes },
        { AffordableHomesAmount.Unknown, (int)he_Pipeline_he_amountofaffordablehomes.Idonotknow },
        { AffordableHomesAmount.Undefined, null },
    };

    public IDictionary<InfrastructureType, int?> Infrastructure => new Dictionary<InfrastructureType, int?>
    {
        { InfrastructureType.SitePreparation, (int)he_pipeline_he_he_infrastructuredoesyourprojectdeliver.Sitepreparation },
        { InfrastructureType.Enabling, (int)he_pipeline_he_he_infrastructuredoesyourprojectdeliver.Enabling },
        { InfrastructureType.PhysicalInfrastructure, (int)he_pipeline_he_he_infrastructuredoesyourprojectdeliver.Physicalinfrastructure },
        { InfrastructureType.Other, (int)he_pipeline_he_he_infrastructuredoesyourprojectdeliver.Other },
        { InfrastructureType.IDoNotKnow, (int)he_pipeline_he_he_infrastructuredoesyourprojectdeliver.Idonotknow },
    };

    public IDictionary<ProjectGeographicFocus, int?> GeographicFocus => new Dictionary<ProjectGeographicFocus, int?>
    {
        { ProjectGeographicFocus.National, (int)he_Pipeline_he_whatisthegeographicfocusoftheproject.National },
        { ProjectGeographicFocus.Regional, (int)he_Pipeline_he_whatisthegeographicfocusoftheproject.Regional },
        { ProjectGeographicFocus.SpecificLocalAuthority, (int)he_Pipeline_he_whatisthegeographicfocusoftheproject.Specificlocalauthority },
        { ProjectGeographicFocus.Unknown, (int)he_Pipeline_he_whatisthegeographicfocusoftheproject.Idonotknow },
        { ProjectGeographicFocus.Undefined, null },
    };

    public IDictionary<RegionType, int?> RegionType => new Dictionary<RegionType, int?>
    {
        { Contract.RegionType.NorthEast, (int)he_pipeline_he_regionlocation.NorthEast },
        { Contract.RegionType.NorthWest, (int)he_pipeline_he_regionlocation.NorthWest },
        { Contract.RegionType.YorkshireAndTheHumber, (int)he_pipeline_he_regionlocation.YorkshireandtheHumber },
        { Contract.RegionType.EastMidlands, (int)he_pipeline_he_regionlocation.EastMidlands },
        { Contract.RegionType.WestMidlands, (int)he_pipeline_he_regionlocation.WestMidlands },
        { Contract.RegionType.EastOfEngland, (int)he_pipeline_he_regionlocation.EastofEngland },
        { Contract.RegionType.SouthEast, (int)he_pipeline_he_regionlocation.SouthEast },
        { Contract.RegionType.SouthWest, (int)he_pipeline_he_regionlocation.SouthWest },
        { Contract.RegionType.London, (int)he_pipeline_he_regionlocation.London },
    };

    public IDictionary<RequiredFundingOption, int?> FundingAmount => new Dictionary<RequiredFundingOption, int?>
    {
        { RequiredFundingOption.LessThan250K, (int)he_Pipeline_he_fundingask.Lessthan250000 },
        { RequiredFundingOption.Between250KAnd1Mln, (int)he_Pipeline_he_fundingask._250000to1million },
        { RequiredFundingOption.Between1MlnAnd5Mln, (int)he_Pipeline_he_fundingask._1millionto5million },
        { RequiredFundingOption.Between5MlnAnd10Mln, (int)he_Pipeline_he_fundingask._5millionto10million },
        { RequiredFundingOption.Between10MlnAnd30Mln, (int)he_Pipeline_he_fundingask._10millionto30million },
        { RequiredFundingOption.Between30MlnAnd50Mln, (int)he_Pipeline_he_fundingask._30millionto50million },
        { RequiredFundingOption.MoreThan50Mln, (int)he_Pipeline_he_fundingask.Morethan50million },
        { RequiredFundingOption.IDoNotKnow, (int)he_Pipeline_he_fundingask.Idontknow },
    };
}
