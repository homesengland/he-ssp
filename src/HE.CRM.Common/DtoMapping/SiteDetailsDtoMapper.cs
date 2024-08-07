using DataverseModel;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;

namespace HE.CRM.Common.DtoMapping
{
    public class SiteDetailsDtoMapper
    {
        public SiteDetailsDtoMapper() { }
        public static invln_SiteDetails MapSiteDetailsDtoToRegularEntity(bool useHeTables, SiteDetailsDto siteDetail, string loanApplicationGuid, invln_localauthority localAuthority = null, he_LocalAuthority heLocalAuthority = null)
        {
            var siteDetailToReturn = new invln_SiteDetails()
            {
                invln_currentvalue = ParseDecimalToMoney(siteDetail.currentValue),
                invln_Dateofpurchase = siteDetail.dateOfPurchase,
                invln_Existinglegalcharges = siteDetail.existingLegalCharges,
                invln_Existinglegalchargesinformation = siteDetail.existingLegalChargesInformation,
                invln_Haveaplanningreferencenumber = siteDetail.haveAPlanningReferenceNumber,
                invln_HowMuch = ParseDecimalToMoney(siteDetail.howMuch),
                invln_Landregistrytitlenumber = siteDetail.landRegistryTitleNumber,
                invln_Name = siteDetail.Name,
                invln_Nameofgrantfund = siteDetail.nameOfGrantFund,
                invln_Numberofhomes = ParseInt(siteDetail.numberOfHomes),
                invln_OtherTypeofhomes = siteDetail.otherTypeOfHomes,
                invln_Planningreferencenumber = siteDetail.planningReferenceNumber,
                invln_Publicsectorfunding = MapPublicSectorFunding(siteDetail.publicSectorFunding),
                invln_Reason = siteDetail.reason,
                invln_Sitecoordinates = siteDetail.siteCoordinates,
                invln_Sitecost = ParseDecimalToMoney(siteDetail.siteCost),
                invln_Sitename = siteDetail.siteName,
                invln_Siteownership = siteDetail.siteOwnership,
                invln_Typeofhomes = MapTypeOfHomes(siteDetail.typeOfHomes),
                invln_TypeofSite = MapTypeOfSite(siteDetail.typeOfSite),
                invln_Valuationsource = MapValuationSource(siteDetail.valuationSource),
                invln_Whoprovided = siteDetail.whoProvided,
                invln_planningpermissionstatus = MapNullableIntToOptionSetValue(siteDetail.planningPermissionStatus),
                invln_startdate = siteDetail.startDate,
                invln_Affordablehousing = siteDetail.affordableHousing,
                invln_completionstatus = MapNullableIntToOptionSetValue(siteDetail.completionStatus),
                invln_projecthasstartdate = siteDetail.projectHasStartDate,
            };
            if (Guid.TryParse(loanApplicationGuid, out Guid applicationId))
            {
                siteDetailToReturn.invln_Loanapplication = new EntityReference(invln_Loanapplication.EntityLogicalName, applicationId);
            }
            if (Guid.TryParse(siteDetail.siteDetailsId, out Guid detailId))
            {
                siteDetailToReturn.Id = detailId;
            }

            if (Guid.TryParse(siteDetail.frontDoorSiteId, out Guid frontDoorSiteId))
            {
                if (useHeTables)
                {
                    siteDetailToReturn.invln_HeProjectLocalAuthorityId = new EntityReference(he_ProjectLocalAuthority.EntityLogicalName, frontDoorSiteId);
                }
                else
                {
                    siteDetailToReturn.invln_FDSiteId = new EntityReference(invln_FrontDoorProjectSitePOC.EntityLogicalName, frontDoorSiteId);
                }
            }

            if (heLocalAuthority != null)
            {
                siteDetailToReturn.invln_HeLocalAuthorityId = heLocalAuthority.ToEntityReference();
                siteDetailToReturn.invln_HERegion = new OptionSetValue(heLocalAuthority.he_laregion.Value);
            }

            if (localAuthority != null)
            {
                siteDetailToReturn.invln_LocalAuthorityID = localAuthority.ToEntityReference();
            }

            return siteDetailToReturn;
        }

        public static SiteDetailsDto MapSiteDetailsToDto(bool useHeTables, invln_SiteDetails siteDetails, invln_localauthority localAuthority = null, he_LocalAuthority heLocalAuthority = null)
        {
            var siteDetailToReturn = new SiteDetailsDto()
            {
                siteDetailsId = siteDetails.invln_SiteDetailsId?.ToString(),
                currentValue = (siteDetails.invln_currentvalue?.Value)?.ToString(),
                dateOfPurchase = siteDetails.invln_Dateofpurchase,
                existingLegalCharges = siteDetails.invln_Existinglegalcharges,
                existingLegalChargesInformation = siteDetails.invln_Existinglegalchargesinformation,
                haveAPlanningReferenceNumber = siteDetails.invln_Haveaplanningreferencenumber,
                howMuch = (siteDetails.invln_HowMuch?.Value)?.ToString(),
                landRegistryTitleNumber = siteDetails.invln_Landregistrytitlenumber,
                Name = siteDetails.invln_Name,
                nameOfGrantFund = siteDetails.invln_Nameofgrantfund,
                numberOfHomes = siteDetails.invln_Numberofhomes?.ToString(),
                otherTypeOfHomes = siteDetails.invln_OtherTypeofhomes,
                planningReferenceNumber = siteDetails.invln_Planningreferencenumber,
                publicSectorFunding = MapPublicSectorFundingOptionSetToString(siteDetails.invln_Publicsectorfunding),
                reason = siteDetails.invln_Reason,
                siteCoordinates = siteDetails.invln_Sitecoordinates,
                siteCost = siteDetails.invln_Sitecost?.Value.ToString(),
                siteName = siteDetails.invln_Sitename,
                siteOwnership = siteDetails.invln_Siteownership,
                typeOfHomes = MapTypeOfHomesOptionSetToString(siteDetails.invln_Typeofhomes),
                typeOfSite = MapTypeOfSiteOptionSetToString(siteDetails.invln_TypeofSite),
                valuationSource = MapValuationSourceOptionSetToString(siteDetails.invln_Valuationsource),
                whoProvided = siteDetails.invln_Whoprovided,
                startDate = siteDetails.invln_startdate,
                planningPermissionStatus = siteDetails.invln_planningpermissionstatus?.Value,
                affordableHousing = siteDetails.invln_Affordablehousing,
                completionStatus = siteDetails.invln_completionstatus?.Value,
                projectHasStartDate = siteDetails.invln_projecthasstartdate,
            };

            if (useHeTables)
            {
                siteDetailToReturn.frontDoorSiteId = siteDetails.invln_HeProjectLocalAuthorityId?.Id.ToString();
                siteDetailToReturn.frontDoorSiteName = siteDetails.invln_HeProjectLocalAuthorityId?.Name;

                if (heLocalAuthority != null)
                {
                    siteDetailToReturn.localAuthority = new LocalAuthorityDto()
                    {
                        name = heLocalAuthority.he_Name,
                        code = heLocalAuthority.he_GSSCode,
                        onsCode = heLocalAuthority.he_GSSCode,
                    };
                }
            }
            else
            {
                siteDetailToReturn.frontDoorSiteId = siteDetails.invln_FDSiteId?.Id.ToString();
                siteDetailToReturn.frontDoorSiteName = siteDetails.invln_FDSiteId?.Name;

                if (localAuthority != null)
                {
                    siteDetailToReturn.localAuthority = new LocalAuthorityDto()
                    {
                        name = localAuthority.invln_localauthorityname.ToString(),
                        code = localAuthority.invln_onscode,
                        onsCode = localAuthority.invln_onscode,
                    };
                }
            }

            return siteDetailToReturn;
        }

        public static OptionSetValueCollection MapTypeOfHomes(string[] typeOfHomes)
        {
            if (typeOfHomes != null && typeOfHomes.Length > 0)
            {
                var collection = new OptionSetValueCollection();
                foreach (var home in typeOfHomes)
                {
                    switch (home?.ToLower())
                    {
                        case "apartmentsorflats":
                            collection.Add(new OptionSetValue((int)invln_Typeofhomes.Apartmentsorflats));
                            break;
                        case "bungalows":
                            collection.Add(new OptionSetValue((int)invln_Typeofhomes.Bungalows));
                            break;
                        case "extracareorassistedliving":
                            collection.Add(new OptionSetValue((int)invln_Typeofhomes.Extracareorassisted));
                            break;
                        case "houses":
                            collection.Add(new OptionSetValue((int)invln_Typeofhomes.Houses));
                            break;
                        case "other":
                            collection.Add(new OptionSetValue((int)invln_Typeofhomes.Other));
                            break;
                    }
                }
                return collection;
            }
            return null;
        }

        public static string[] MapTypeOfHomesOptionSetToString(OptionSetValueCollection typeOfHomes)
        {
            if (typeOfHomes != null && typeOfHomes.Count > 0)
            {
                List<string> collection = new List<string>();
                foreach (var home in typeOfHomes)
                {
                    switch (home?.Value)
                    {
                        case (int)invln_Typeofhomes.Apartmentsorflats:
                            collection.Add("apartmentsorflats");
                            break;
                        case (int)invln_Typeofhomes.Bungalows:
                            collection.Add("bungalows");
                            break;
                        case (int)invln_Typeofhomes.Extracareorassisted:
                            collection.Add("extracareorassistedliving");
                            break;
                        case (int)invln_Typeofhomes.Houses:
                            collection.Add("houses");
                            break;
                        case (int)invln_Typeofhomes.Other:
                            collection.Add("other");
                            break;
                    }
                }
                return collection.ToArray();
            }
            return null;
        }

        public static OptionSetValue MapPublicSectorFunding(string publicSectorFunding)
        {
            switch (publicSectorFunding?.ToLower())
            {
                case "no":
                    return new OptionSetValue((int)invln_Publicsectorfunding.No);
                case "donotknow":
                    return new OptionSetValue((int)invln_Publicsectorfunding.Donotknow);
                case "yes":
                    return new OptionSetValue((int)invln_Publicsectorfunding.Yes);
            }

            return null;
        }

        public static string MapPublicSectorFundingOptionSetToString(OptionSetValue publicSectorFunding)
        {
            switch (publicSectorFunding?.Value)
            {
                case (int)invln_Publicsectorfunding.No:
                    return "no";
                case (int)invln_Publicsectorfunding.Donotknow:
                    return "donotknow";
                case (int)invln_Publicsectorfunding.Yes:
                    return "yes";
            }

            return null;
        }

        public static OptionSetValue MapValuationSource(string valuationSource)
        {
            switch (valuationSource?.ToLower())
            {
                case "customerestimate":
                    return new OptionSetValue((int)invln_Valuationsource.Customerestimate);
                case "ricsredbookvaluation":
                    return new OptionSetValue((int)invln_Valuationsource.RICSRedBookvaluation);
                case "estateagentestimate":
                    return new OptionSetValue((int)invln_Valuationsource.Estateagentestimate);
            }

            return null;
        }

        public static string MapValuationSourceOptionSetToString(OptionSetValue valuationSource)
        {
            switch (valuationSource?.Value)
            {
                case (int)invln_Valuationsource.Customerestimate:
                    return "customerestimate";
                case (int)invln_Valuationsource.RICSRedBookvaluation:
                    return "ricsredbookvaluation";
                case (int)invln_Valuationsource.Estateagentestimate:
                    return "estateagentestimate";
            }

            return null;
        }

        public static OptionSetValue MapTypeOfSite(string typeOfSite)
        {
            switch (typeOfSite?.ToLower())
            {
                case "greenfield":
                    return new OptionSetValue((int)invln_TypeofSite.Greenfield);
                case "brownfield":
                    return new OptionSetValue((int)invln_TypeofSite.Brownfield);
            }

            return null;
        }

        public static string MapTypeOfSiteOptionSetToString(OptionSetValue typeOfSite)
        {
            switch (typeOfSite?.Value)
            {
                case (int)invln_TypeofSite.Greenfield:
                    return "greenfield";
                case (int)invln_TypeofSite.Brownfield:
                    return "brownfield";
            }

            return null;
        }

        public static OptionSetValue MapNullableIntToOptionSetValue(int? valueToMap)
        {
            if (valueToMap.HasValue)
            {
                return new OptionSetValue(valueToMap.Value);
            }
            return null;
        }

        private static int? ParseInt(string intToParse)
        {
            if (int.TryParse(intToParse, out int intValue))
            {
                return intValue;
            }
            else
            {
                return null;
            }
        }

        private static Money ParseDecimalToMoney(string decimalToParse)
        {
            if (decimal.TryParse(decimalToParse, out decimal decimalValue))
            {
                return new Money(decimalValue);
            }
            else
            {
                return null;
            }
        }

        private static bool? ParseBool(string boolToParse)
        {
            switch (boolToParse?.ToLower())
            {
                case "yes":
                    return true;
                case "no":
                    return false;
            }

            if (bool.TryParse(boolToParse, out bool boolValue))
            {
                return boolValue;
            }
            else
            {
                return null;
            }
        }
    }
}
