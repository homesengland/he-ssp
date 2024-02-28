using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Services;
using DataverseModel;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.DtoMapping
{
    public class HomeTypeMapper
    {
        public static HomeTypeDto MapRegularEntityToDto(invln_HomeType homeType)
        {
            var homeTypeDto = new HomeTypeDto()
            {
                homeTypeName = homeType.invln_hometypename,
                housingType = homeType.invln_typeofhousing?.Value,
                numberOfHomes = homeType.invln_numberofhomeshometype,
                housingTypeForOlderPeople = homeType.invln_typeofolderpeopleshousing?.Value,
                housingTypeForVulnerable = homeType.invln_typeofhousingfordisabledvulnerablepeople?.Value,
                clientGroup = homeType.invln_clientgroup?.Value,
                designPrinciples = new List<int>(),
                localComissioningBodies = homeType.invln_localcommissioningbodiesconsulted,
                shortStayAccommodation = homeType.invln_homesusedforshortstay,
                revenueFunding = homeType.invln_revenuefunding?.Value,
                moveOnArrangements = homeType.invln_moveonarrangementsforshortstayhomes,
                fundingSources = new List<int>(),
                exitPlan = homeType.invln_supportedhousingexitplan,
                typologyLocationAndDesign = homeType.invln_typologylocationanddesing,
                numberOfBedrooms = homeType.invln_numberofbedrooms,
                maxOccupancy = homeType.invln_maxoccupancy,
                numberOfStoreys = homeType.invln_numberofstoreys,
                createdOn = homeType.CreatedOn,
                buildingType = homeType.invln_buildingtype?.Value,
                sharedFacilities = homeType.invln_facilities?.Value,
                isMoveOnAccommodation = homeType.invln_homesusedformoveonaccommodation,
                needsOfParticularGroup = homeType.invln_homesdesignedforuseofparticulargroup,
                homesDesignedForUseOfParticularGroup = homeType.invln_homesdesignedforuseofparticular?.Value,
                areHomesCustomBuild = homeType.invln_custombuild,
                accessibilityCategory = homeType.invln_accessibilitycategory?.Value,
                marketValue = homeType.invln_MarketValueofEachProperty?.Value, //before invln_marketvalue
                marketRent = homeType.invln_MarketRentperWeek?.Value, //before invln_marketrent
                prospectiveRent = homeType.invln_ProspectiveRentperWeek?.Value, //before invln_prospectiverent
                isWheelchairStandardMet = homeType.invln_iswheelchairstandardmet,
                designPlansMoreInformation = homeType.invln_designplancomments,
                RtSOExemption = homeType.invln_rtsoexempt,
                initialSalePercent = homeType.invln_SharedOwnershipInitialSale, //before invln_initialsale
                prospectiveRentAsPercentOfMarketRent = homeType.invln_prospectiverentasofmarketrent,
                isCompleted = homeType.invln_ishometypecompleted,
                exemptionJustification = homeType.invln_reasonsforrtsoexemption,
                floorArea = homeType.invln_floorarea,
                doAllHomesMeetNDSS = homeType.invln_doallhomesmeetNDSS,
                whichNDSSStandardsHaveBeenMet = new List<int>(),
                targetRentOver80PercentOfMarketRent = homeType.invln_targetrentover80ofmarketrent,
                proposedRentAsPercentOfUnsoldShare = homeType.invln_proposedrentasaofunsoldshare,
                expectedFirstTrancheSaleReceipt = homeType.invln_FirstTrancheSalesReceipt?.Value, //before invln_expectedfirsttranchesalereceipt
                mmcApplied = homeType.invln_mmcapplied,
                mmcCategories = new List<int>(),
                mmcCategories_1subcategories = new List<int>(),
                mmcCategories_2subcategories =new List<int>()
            };

            if (homeType.invln_mmccategories != null && homeType.invln_mmccategories.Any())
            {
                foreach (var category in homeType.invln_mmccategories)
                {
                    homeTypeDto.mmcCategories.Add(category.Value);
                }
            }

            if (homeType.invln_mmccategory1subcategories != null && homeType.invln_mmccategory1subcategories.Any())
            {
                foreach (var firstSubcategory in homeType.invln_mmccategory1subcategories)
                {
                    homeTypeDto.mmcCategories_1subcategories.Add(firstSubcategory.Value);
                }
            }

            if (homeType.invln_mmccategory2subcategories != null && homeType.invln_mmccategory2subcategories.Any())
            {
                foreach (var secondSubcategory in homeType.invln_mmccategory2subcategories)
                {
                    homeTypeDto.mmcCategories_1subcategories.Add(secondSubcategory.Value);
                }
            }

            if (homeType.Id != null)
            {
                homeTypeDto.id = homeType.Id.ToString();
            }

            if (homeType.invln_application?.Id != null)
            {
                homeTypeDto.applicationId = homeType.invln_application.Id.ToString();
            }

            if (homeType.invln_revenuefundingsources != null && homeType.invln_revenuefundingsources.Any())
            {
                foreach (var revenueFundingSource in homeType.invln_revenuefundingsources)
                {
                    homeTypeDto.fundingSources.Add(revenueFundingSource.Value);
                }
            }

            if (homeType.invln_happiprinciples != null && homeType.invln_happiprinciples.Any())
            {
                foreach (var principle in homeType.invln_happiprinciples)
                {
                    homeTypeDto.designPrinciples.Add(principle.Value);
                }
            }

            if (homeType.invln_whichndssstandardshavebeenmet != null && homeType.invln_whichndssstandardshavebeenmet.Any())
            {
                foreach (var standard in homeType.invln_whichndssstandardshavebeenmet)
                {
                    homeTypeDto.whichNDSSStandardsHaveBeenMet.Add(standard.Value);
                }
            }
            return homeTypeDto;
        }

        public static invln_HomeType MapDtoToRegularEntity(HomeTypeDto homeTypeDto, string applicationId = null)
        {
            var homeType = new invln_HomeType()
            {
                invln_hometypename = homeTypeDto.homeTypeName,
                invln_typeofhousing = MapNullableIntToOptionSetValue(homeTypeDto.housingType),
                invln_numberofhomeshometype = homeTypeDto.numberOfHomes,
                invln_typeofolderpeopleshousing = MapNullableIntToOptionSetValue(homeTypeDto.housingTypeForOlderPeople),
                invln_typeofhousingfordisabledvulnerablepeople = MapNullableIntToOptionSetValue(homeTypeDto.housingTypeForVulnerable),
                invln_clientgroup = MapNullableIntToOptionSetValue(homeTypeDto.clientGroup),
                invln_happiprinciples = new OptionSetValueCollection(),
                invln_localcommissioningbodiesconsulted = homeTypeDto.localComissioningBodies,
                invln_homesusedforshortstay = homeTypeDto.shortStayAccommodation,
                invln_revenuefunding = MapNullableIntToOptionSetValue(homeTypeDto.revenueFunding),
                invln_moveonarrangementsforshortstayhomes = homeTypeDto.moveOnArrangements,
                invln_revenuefundingsources = new OptionSetValueCollection(),
                invln_supportedhousingexitplan = homeTypeDto.exitPlan,
                invln_typologylocationanddesing = homeTypeDto.typologyLocationAndDesign,
                invln_numberofbedrooms = homeTypeDto.numberOfBedrooms,
                invln_maxoccupancy = homeTypeDto.maxOccupancy,
                invln_numberofstoreys = homeTypeDto.numberOfStoreys,
                invln_buildingtype = MapNullableIntToOptionSetValue(homeTypeDto.buildingType),
                invln_facilities = MapNullableIntToOptionSetValue(homeTypeDto.sharedFacilities),
                invln_homesusedformoveonaccommodation = homeTypeDto.isMoveOnAccommodation,
                invln_homesdesignedforuseofparticulargroup = homeTypeDto.needsOfParticularGroup,
                invln_homesdesignedforuseofparticular = MapNullableIntToOptionSetValue(homeTypeDto.homesDesignedForUseOfParticularGroup),
                invln_custombuild = homeTypeDto.areHomesCustomBuild,
                invln_accessibilitycategory = MapNullableIntToOptionSetValue(homeTypeDto.accessibilityCategory),
                invln_MarketValueofEachProperty = MapNullableDecimalToMoney(homeTypeDto.marketValue),
                invln_MarketRentperWeek = MapNullableDecimalToMoney(homeTypeDto.marketRent),
                invln_ProspectiveRentperWeek = MapNullableDecimalToMoney(homeTypeDto.prospectiveRent),
                invln_iswheelchairstandardmet = homeTypeDto.isWheelchairStandardMet,
                invln_designplancomments = homeTypeDto.designPlansMoreInformation,
                invln_rtsoexempt = homeTypeDto.RtSOExemption,
                invln_SharedOwnershipInitialSale = homeTypeDto.initialSalePercent,
                invln_prospectiverentasofmarketrent = homeTypeDto.prospectiveRentAsPercentOfMarketRent,
                invln_ishometypecompleted = homeTypeDto.isCompleted,
                invln_reasonsforrtsoexemption = homeTypeDto.exemptionJustification,
                invln_floorarea = homeTypeDto.floorArea,
                invln_doallhomesmeetNDSS = homeTypeDto.doAllHomesMeetNDSS,
                invln_whichndssstandardshavebeenmet = new OptionSetValueCollection(),
                invln_targetrentover80ofmarketrent = homeTypeDto.targetRentOver80PercentOfMarketRent,
                invln_FirstTrancheSalesReceipt = MapNullableDecimalToMoney(homeTypeDto.expectedFirstTrancheSaleReceipt),
                invln_proposedrentasaofunsoldshare = homeTypeDto.proposedRentAsPercentOfUnsoldShare,
                invln_mmcapplied = homeTypeDto.mmcApplied,
                invln_mmccategories = new OptionSetValueCollection(),//new OptionSetValueCollection(homeTypeDto.mmcCategories.Select(x => new OptionSetValue(x)).ToList()),
                invln_mmccategory1subcategories = new OptionSetValueCollection(),//new OptionSetValueCollection(homeTypeDto.mmcCategories_1subcategories.Select(x => new OptionSetValue(x)).ToList()),
                invln_mmccategory2subcategories = new OptionSetValueCollection(),//new OptionSetValueCollection(homeTypeDto.mmcCategories_2subcategories.Select(x => new OptionSetValue(x)).ToList()),
            };

            if (homeTypeDto.mmcCategories != null && homeTypeDto.mmcCategories.Any())
            {
                foreach (var category in homeTypeDto.mmcCategories)
                {
                    homeType.invln_mmccategories.Add(new OptionSetValue(category));
                }
            }

            if (homeTypeDto.mmcCategories_1subcategories != null && homeTypeDto.mmcCategories_1subcategories.Any())
            {
                foreach (var firstSubagegory in homeTypeDto.mmcCategories_1subcategories)
                {
                    homeType.invln_mmccategory1subcategories.Add(new OptionSetValue(firstSubagegory));
                }
            }

            if (homeTypeDto.mmcCategories_2subcategories != null && homeTypeDto.mmcCategories_2subcategories.Any())
            {
                foreach (var secondSubcategory in homeTypeDto.mmcCategories_2subcategories)
                {
                    homeType.invln_mmccategory2subcategories.Add(new OptionSetValue(secondSubcategory));
                }
            }

            if (homeTypeDto.id != null)
            {
                homeType.Id = new Guid(homeTypeDto.id);
            }

            if (Guid.TryParse(applicationId ?? homeTypeDto.applicationId, out var applicationGuid))
            {
                homeType.invln_application = new EntityReference(invln_scheme.EntityLogicalName, applicationGuid);
            }

            if (homeTypeDto.fundingSources != null && homeTypeDto.fundingSources.Any())
            {
                foreach (var fundingSource in homeTypeDto.fundingSources)
                {
                    homeType.invln_revenuefundingsources.Add(new OptionSetValue(fundingSource));
                }
            }

            if (homeTypeDto.designPrinciples != null && homeTypeDto.designPrinciples.Any())
            {
                foreach (var principle in homeTypeDto.designPrinciples)
                {
                    homeType.invln_happiprinciples.Add(new OptionSetValue(principle));
                }
            }

            if (homeTypeDto.whichNDSSStandardsHaveBeenMet != null && homeTypeDto.whichNDSSStandardsHaveBeenMet.Any())
            {
                foreach (var principle in homeTypeDto.whichNDSSStandardsHaveBeenMet)
                {
                    homeType.invln_whichndssstandardshavebeenmet.Add(new OptionSetValue(principle));
                }
            }

            return homeType;
        }

        private static OptionSetValue MapNullableIntToOptionSetValue(int? valueToMap)
        {
            if (valueToMap.HasValue)
            {
                return new OptionSetValue(valueToMap.Value);
            }
            return null;
        }
        private static Money MapNullableDecimalToMoney(decimal? valueToMap)
        {
            if (valueToMap.HasValue)
            {
                return new Money(valueToMap.Value);
            }
            return null;
        }
    }
}
