using DataverseModel;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System.Configuration;
using System.Text.Json;
using Microsoft.PowerPlatform.Dataverse.Client.Extensions;

namespace HE.Xrm.ServiceClientExample
{
    internal class Program
    {
        public class AppSettings
        {
            public string Url { get; set; }
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
        }

        static void Main(string[] args)
        {
            var connectionString = "";
            using (var reader = new StreamReader(Directory.GetCurrentDirectory() + "../../../../appsettings.json"))
            {
                var appSettings = JsonSerializer.Deserialize<AppSettings>(reader.ReadToEnd());
                if (appSettings != null)
                {
                    connectionString = $@"AuthType = ClientSecret; ClientId={appSettings.ClientId};Url={appSettings.Url};ClientSecret={appSettings.ClientSecret};"; //crm connection string
                }
                else
                {
                    throw new ConfigurationException("Missing configuration");
                }
            }

            using (ServiceClient serviceClient = new(connectionString))
            {
                if (serviceClient.IsReady)
                {
                    TestChangeStatus(serviceClient);
                    //TestCustomApiCallingPath(serviceClient);
                    //TestUpdateLoanApplication(serviceClient); //method to call
                }
                else
                {
                    Console.WriteLine("A web service connection was not established.");
                }
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        private static void TestChangeStatus(ServiceClient serviceClient)
        {
            var req1 = new invln_getahpapplicationdocumentlocationRequest()
            {
                invln_applicationid = "7ad49e6a-b495-ee11-be37-002248004f63"
            };
             var test = serviceClient.Execute(req1);
            Console.WriteLine("A web service connection was not established.");
        }

        private static void TestHomeType(ServiceClient serviceClient)
        {
            var fieldsToUpdate = $"{nameof(invln_HomeType.invln_hometypename).ToLower()},{nameof(invln_HomeType.invln_typeofhousing).ToLower()}";
            var home = new HomeTypeDto()
            {
                id = "7a34a4f9-1689-ee11-be36-0022480041cf",
                numberOfHomes = 222,
                applicationId = "a5b507b3-c27f-ee11-8179-002248004268",
                homeTypeName = "test adda",
                housingType = (int)invln_Typeofhousing.Housingfordisabledandvulnerablepeople,
                housingTypeForOlderPeople = (int)invln_typeofolderpeopleshousing.Housingforolderpeoplewithallspecialdesignfeatures,
                housingTypeForVulnerable = (int)invln_typeofhousingfordisabledvulnerable.Designatedhousingfordisabledandvulnerablepeoplewithaccesstosupport,
                clientGroup = (int)invln_Clientgroup.DPeoplewithdrugproblems,
                designPrinciples = new List<int>() { (int)invln_HAPPIprinciples.DEnergyefficiencyandsustainabledesign, (int)invln_HAPPIprinciples.GPositiveuseofcirculationspace },
            };
            var serialized = JsonSerializer.Serialize(home);
            var req2 = new invln_sethometypeRequest() //get loan applications related to account and contact with given data
            {
                invln_hometype = serialized,
                invln_fieldstoset = "invln_hometypename, invln_typeofhousing",
                invln_organisationid = "db0da27f-bcfb-ed11-8f6d-002248c653e1",
                invln_userid = "auth0|6548d4ef9e1110e85f1fef57",
                invln_applicationid = "49ab97e2-f882-ee11-8179-002248004f63",
            };
            var req3 = new invln_deletehometypeRequest()
            {
                invln_hometypeid = "7a34a4f9-1689-ee11-be36-0022480041cf",
                invln_organisationid = "db0da27f-bcfb-ed11-8f6d-002248c653e1",
                invln_userid = "auth0|6548d4ef9e1110e85f1fef57",
                invln_applicationid = "49ab97e2-f882-ee11-8179-002248004f63",
            };
            serviceClient.Execute(req3);
            Console.WriteLine("A web service connection was not established.");
        }

        private static void TestLoan(ServiceClient serviceClient)
        {
            var applicationDtoToReturn = new AhpApplicationDto()
            {
                name = "custom api testyttt",
                tenure = (int)invln_Tenure.Renttobuy,
                schemeInformationSectionCompletionStatus = (int)invln_AHPSectioncompletionstatusSet.InProgress,
                homeTypesSectionCompletionStatus = (int)invln_AHPSectioncompletionstatusSet.InProgress,
                financialDetailsSectionCompletionStatus = (int)invln_AHPSectioncompletionstatusSet.InProgress,
                deliveryPhasesSectionCompletionStatus = (int)invln_AHPSectioncompletionstatusSet.InProgress,
                borrowingAgainstRentalIncomeFromThisScheme = 122,
                fundingFromOpenMarketHomesOnThisScheme = 7777,
                fundingFromOpenMarketHomesNotOnThisScheme = 6969,
                //fundingGeneratedFromOtherSources = application.?.Value,
                recycledCapitalGrantFund = 2137,
                //transferValue = application.?.Value,
                //totalInitialSalesIncome = application.?.Value,
                //otherCapitalSources = application.?.Value,
                id = "49ab97e2-f882-ee11-8179-002248004f63",
                fundingRequested = 3333,
                noOfHomes = 123,
                affordabilityEvidence = "12312321",
                discussionsWithLocalStakeholders = "ddasdasddsad",
                meetingLocalHousingNeed = "housing need",
                meetingLocalProrities = "2123dsadsa",
                reducingEnvironmentalImpact = "yes",
                sharedOwnershipSalesRisk = "ssssss"
            };
            var fieldsToUpdate = $"{nameof(invln_scheme.invln_totalhomes).ToLower()},{nameof(invln_scheme.invln_Tenure).ToLower()}," +
                $"{nameof(invln_scheme.invln_schemeinformationsectioncompletionstatus).ToLower()},{nameof(invln_scheme.invln_deliveryphasessectioncompletionstatus).ToLower()}," +
                $"{nameof(invln_scheme.invln_financialdetailssectioncompletionstatus).ToLower()},{nameof(invln_scheme.invln_hometypessectioncompletionstatus).ToLower()}," +
                $"{nameof(invln_scheme.invln_borrowingagainstrentalincome).ToLower()},{nameof(invln_scheme.invln_fundingfromopenmarkethomesonthisscheme).ToLower()}," +
                $"{nameof(invln_scheme.invln_fundingfromopenmarkethomesnotonthisscheme).ToLower()},{nameof(invln_scheme.invln_recycledcapitalgrantfund).ToLower()}," +
                $"{nameof(invln_scheme.invln_borrowingagainstrentalincome).ToLower()},{nameof(invln_scheme.invln_fundingfromopenmarkethomesonthisscheme).ToLower()}," +
                $"{nameof(invln_scheme.invln_fundingrequired).ToLower()},{nameof(invln_scheme.invln_noofhomes).ToLower()}," +
                $"{nameof(invln_scheme.invln_discussionswithlocalstakeholders).ToLower()},{nameof(invln_scheme.invln_meetinglocalhousingneed).ToLower()}," +
                $"{nameof(invln_scheme.invln_meetinglocalpriorities).ToLower()},{nameof(invln_scheme.invln_reducingenvironmentalimpact).ToLower()}," +
                $"{nameof(invln_scheme.invln_schemename).ToLower()},{nameof(invln_scheme.invln_lastexternalmodificationby).ToLower()},{nameof(invln_scheme.invln_lastexternalmodificationon).ToLower()}";
            var app = JsonSerializer.Serialize(applicationDtoToReturn);
            var req2 = new invln_setahpapplicationRequest() //get loan applications related to account and contact with given data
            {
                invln_application = app,
                invln_organisationid = "db0da27f-bcfb-ed11-8f6d-002248c653e1",
                invln_userid = "auth0|6548d4ef9e1110e85f1fef57",
                invln_fieldstoupdate = fieldsToUpdate,
            };
           // var resp2 = (invln_setahpapplicationResponse)serviceClient.Execute(req2);
            var req3 = new invln_getahpapplicationRequest() //get loan applications related to account and contact with given data
            {
                invln_appfieldstoretrieve = fieldsToUpdate,
                invln_applicationid = "49ab97e2-f882-ee11-8179-002248004f63",
                invln_organisationid = "db0da27f-bcfb-ed11-8f6d-002248c653e1",
                invln_userid = "auth0|6548d4ef9e1110e85f1fef57",
            };
            var resp3 = (invln_getahpapplicationResponse)serviceClient.Execute(req3);

            Console.WriteLine("A web service connection was not established.");
        }

        private static void TestQuery(ServiceClient service)
        {
            string organisationNames = "pwc ass";
            IEnumerable<string> names = organisationNames.Split(' ').ToList();
            bool recordsWithoutCopanyNumberIncluded = false;
            var filter1 = new FilterExpression
            {
                FilterOperator = LogicalOperator.And,
            };

            var cols = new ColumnSet("name", "he_companieshousenumber", "address1_line1", "address1_line2",
                "address1_line3", "address1_city", "address1_postalcode", "address1_country");

            var query = new QueryExpression("account")
            {
                ColumnSet = cols,
            };

            var numberOfRequestsInQuery = 1;
            var recordsWithoutCompanyNumberFilter = new FilterExpression();
            if (!recordsWithoutCopanyNumberIncluded)
            {
                recordsWithoutCompanyNumberFilter = new FilterExpression
                {
                    FilterOperator = LogicalOperator.And,
                    Conditions =
                {
                    new ConditionExpression("he_companieshousenumber", ConditionOperator.NotNull),
                },
                };
            }

            var retrievedEntitiesCollection = new EntityCollection();
            EntityCollection retrievedEntities;
            foreach (var name in names)
            {
                var condition1 = new ConditionExpression("name", ConditionOperator.Like, $"%{name}%");
                filter1.Conditions.Add(condition1);
                numberOfRequestsInQuery++;
                if (numberOfRequestsInQuery >= 490)
                {
                    numberOfRequestsInQuery = 0;
                    query.Criteria.AddFilter(filter1);

                    retrievedEntities = service.RetrieveMultiple(query);
                    if (retrievedEntities != null)
                    {
                        retrievedEntitiesCollection.Entities.AddRange(retrievedEntities.Entities);
                    }

                    query.Criteria.AddFilter(recordsWithoutCompanyNumberFilter);

                    filter1.Conditions.Clear();
                    query.Criteria.Filters.Clear();
                }
            }

            query.Criteria.AddFilter(filter1);
            query.Criteria.AddFilter(recordsWithoutCompanyNumberFilter);

            retrievedEntities = service.RetrieveMultiple(query);
            if (retrievedEntities != null)
            {
                retrievedEntitiesCollection.Entities.AddRange(retrievedEntities.Entities);
            }

            var organizationDtoList = new List<OrganizationDetailsDto>();
            foreach (var account in retrievedEntitiesCollection.Entities)
            {
                var organization = new OrganizationDetailsDto()
                {
                    registeredCompanyName = account.Contains("name") ? account["name"].ToString() : null,
                    companyRegistrationNumber = account.Contains("he_companieshousenumber") ? account["he_companieshousenumber"].ToString() : null,
                    addressLine1 = account.Contains("address1_line1") ? account["address1_line1"].ToString() : null,
                    addressLine2 = account.Contains("address1_line2") ? account["address1_line2"].ToString() : null,
                    addressLine3 = account.Contains("address1_line3") ? account["address1_line3"].ToString() : null,
                    city = account.Contains("address1_city") ? account["address1_city"].ToString() : null,
                    postalcode = account.Contains("address1_postalcode") ? account["address1_postalcode"].ToString() : null,
                    country = account.Contains("address1_country") ? account["address1_country"].ToString() : null,
                    organisationId = account.Contains("accountid") ? account["accountid"].ToString() : null,
                };
                organizationDtoList.Add(organization);
            }
            Console.WriteLine("test end.");
        }

        private static void UserProfileTest(ServiceClient serviceClient)
        {
            var req1 = new invln_returnuserprofileRequest()
            {
                invln_contactexternalid = "auth0|64a686b77d88c88f0580d8d1"
            };

            var resp1 = (invln_returnuserprofileResponse)serviceClient.Execute(req1);
            var resp1Deserialized = JsonSerializer.Deserialize<ContactDto>(resp1.invln_userprofile);


            var req2 = new invln_updateuserprofileRequest()
            {
                invln_contactexternalid = req1.invln_contactexternalid,
                invln_contact = JsonSerializer.Serialize(resp1Deserialized),
            };
            serviceClient.Execute(req2);
            Console.WriteLine("test end.");
        }

        private static void QuickTest(ServiceClient serviceClient)
        {
            var req1 = new invln_getcontactroleRequest() //get contact role
            {
                invln_contactemail = "",
                invln_contactexternalid = "auth0|64a28c7fb67ed30b288d6ff7",
                invln_portaltype = ((int)invln_Portal1.Loans).ToString(),
            };

            var resp1 = (invln_getcontactroleResponse)serviceClient.Execute(req1);
            var resp1Deserialized = JsonSerializer.Deserialize<ContactRolesDto>(resp1.invln_portalroles);

            var req10 = new invln_sendinvestmentloansdatatocrmRequest() //create or update loan application and create or update related site details
            {
                invln_contactexternalid = resp1Deserialized.externalId, //contact external id
                //invln_loanapplicationid = resp2Element.loanApplicationId, //loan app id
                invln_accountid = resp1Deserialized.contactRoles[0].accountId.ToString(), //account
                //invln_entityfieldsparameters = JsonSerializer.Serialize(new LoanApplicationDto())
                invln_entityfieldsparameters = JsonSerializer.Serialize(new LoanApplicationDto()
                {
                    LoanApplicationContact = new UserAccountDto() { ContactEmail = "abcd@aa.cc", ContactExternalId = "auth0|64a28c7fb67ed30b288d6fggijk", ContactFirstName = "Ala", ContactLastName = "Test", AccountId = resp1Deserialized.contactRoles[0].accountId, ContactTelephoneNumber = "8888" },
                }), //serialized Loan Application DTO
            };

            var resp10 = (invln_sendinvestmentloansdatatocrmResponse)serviceClient.Execute(req10);

            //var req112 = new invln_getsingleloanapplicationforaccountandcontactRequest() //get single loan application
            //{
            //    invln_accountid = "429d11ab-15fe-ed11-8f6c-002248c653e1", //accountid
            //    invln_externalcontactid = "auth0|64a28c7fb67ed30b288d6ff7", //external contact id
            //    invln_loanapplicationid = "77505f38-882b-ee11-9965-002248c653e1" //loan application id
            //};
            //var resp112 = (invln_getsingleloanapplicationforaccountandcontactResponse)serviceClient.Execute(req11);

            var req2 = new invln_getloanapplicationsforaccountandcontactRequest() //get loan applications related to account and contact with given data
            {
                invln_accountid = "429d11ab-15fe-ed11-8f6c-002248c653e1", //account id
                invln_externalcontactid = "auth0|64a28c7fb67ed30b288d6fggijk", // contact external id
            };
            var resp2 = (invln_getloanapplicationsforaccountandcontactResponse)serviceClient.Execute(req2);
        }

        private static void TestUpdateLoanApplicationExternalStatus(ServiceClient serviceClient)
        {
            //change loan application status
            var req4 = new invln_changeloanapplicationexternalstatusRequest()
            {
                invln_loanapplicationid = "1d01e285-9915-ee11-9cbe-002248c652b4", //loan id
                invln_statusexternal = 858110009, // external status as int
            };

            serviceClient.Execute(req4);
        }

        private static void TestDeleteRecord(ServiceClient serviceClient)
        {
            //At the moment not in main
            //var req1 = new invln_deleteloanapplicationRequest()
            //{
            //    invln_loanapplicationid = "988d6645-9e24-ee11-9965-002248c653e1"
            //};
            //var req1 = new invln_deletesitedetailsRequest()
            //{
            //    invln_sitedetailsid = "3f7464dc-6f0f-ee11-8f6e-002248c653e1"
            //};
            //serviceClient.Execute(req1);
        }

        private static void TestUpdateLoanApplication(ServiceClient serviceClient)
        {

            var loanApp = new LoanApplicationDto() //new fields values
            {
                additionalProjects = true,
                name = "name pluginddd",
                numberOfSites = "27",
                fundingReason = "buildinginfrastructureonly",
                loanApplicationStatus = null,
            };


            var req1 = new invln_updatesingleloanapplicationRequest() //api is not updating related site details, for site details we have another api
            {
                invln_loanapplicationid = "1d01e285-9915-ee11-9cbe-002248c652b4", //application to update
                invln_fieldstoupdate = "invln_name,invln_additionalprojects,invln_fundingreason,invln_externalstatus,invln_numberofsites", //columns to modify
                invln_accountid = "429d11ab-15fe-ed11-8f6c-002248c653e1", // account related to loan application
                invln_contactexternalid = "auth0|64a28c7fb67ed30b288d6ff7", // contact external id related to loan applicaiton
                invln_loanapplication = JsonSerializer.Serialize(loanApp), // loan application serialized DTO data
            };
            serviceClient.Execute(req1);
        }

        private static void TestUpdateSiteDetails(ServiceClient serviceClient)
        {
            var siteDetailsNewValues = new SiteDetailsDto() //fields to update
            {
                landRegistryTitleNumber = "land registry numbzxasadasd",
                currentValue = null,
                Name = "Name",
                dateOfPurchase = DateTime.Now,
                existingLegalCharges = true,
                existingLegalChargesInformation = "info",
                haveAPlanningReferenceNumber = true,
                howMuch = "2137",
                nameOfGrantFund = "name of grand fund",
                numberOfAffordableHomes = "3223",
                numberOfHomes = "4343",
                otherTypeOfHomes = "other",
                planningReferenceNumber = "11112233",
                publicSectorFunding = "no",
                reason = "reason",
                siteCoordinates = "122",
                siteCost = "33341",
                siteName = "Name site",
                siteOwnership = true,
                typeOfHomes = new string[] { "apartmentsorflats", "houses" },
                typeOfSite = "greenfield",
                valuationSource = "estateagentestimate",
                whoProvided = "provided",
            };
            var req1 = new invln_updatesinglesitedetailsRequest()
            {
                invln_sitedetail = "{\"siteDetailsId\":\"1442e3bc-0668-ee11-9ae6-002248004a06\",\"currentValue\":null,\"dateOfPurchase\":null,\"existingLegalCharges\":false,\"existingLegalChargesInformation\":\"\",\"haveAPlanningReferenceNumber\":false,\"howMuch\":null,\"landRegistryTitleNumber\":\"s\",\"Name\":\"asd\",\"nameOfGrantFund\":null,\"numberOfAffordableHomes\":\"No\",\"numberOfHomes\":\"12\",\"otherTypeOfHomes\":null,\"planningReferenceNumber\":null,\"publicSectorFunding\":\"No\",\"reason\":null,\"siteCoordinates\":null,\"siteCost\":null,\"siteName\":null,\"siteOwnership\":false,\"typeOfHomes\":[\"extracareorassistedliving\"],\"typeOfSite\":\"brownfield\",\"valuationSource\":null,\"whoProvided\":null,\"planningPermissionStatus\":null,\"startDate\":null,\"affordableHousing\":false,\"completionStatus\":858110001,\"projectHasStartDate\":false,\"loanApplicationStatus\":null,\"localAuthority\":{\"onsCode\":\"E08000003\",\"name\":\"Manchester\"}}", //site details serialized DTO data
                invln_loanapplicationid = "c541e3bc-0668-ee11-9ae6-002248004a06", // related loan application
                invln_sitedetailsid = "1442e3bc-0668-ee11-9ae6-002248004a06",
                invln_fieldstoupdate= "invln_name,invln_affordablehousing,invln_planningpermissionstatus,invln_haveaplanningreferencenumber,invln_planningreferencenumber,invln_sitecoordinates,invln_landregistrytitlenumber,invln_localauthorityid,invln_siteownership,invln_numberofhomes,invln_typeofsite,invln_typeofhomes,invln_othertypeofhomes,invln_projecthasstartdate,invln_startdate,invln_dateofpurchase,invln_sitecost,invln_currentvalue,invln_valuationsource,invln_publicsectorfunding,invln_whoprovided,invln_reason,invln_howmuch,invln_nameofgrantfund,invln_existinglegalcharges,invln_existinglegalchargesinformation,invln_completionstatus",
            };
            serviceClient.Execute(req1);
        }

        private static void TestCustomApiCallingPath(ServiceClient serviceClient)
        {
            var req1 = new invln_getcontactroleRequest() //get contact role
            {
                invln_contactemail = "abc5@wp.pll",
                invln_contactexternalid = "auth0|64a28c7fb67ed30b288d6fggijk",
                invln_portaltype = ((int)invln_Portal1.Loans).ToString(),
            };

            var resp1 = (invln_getcontactroleResponse)serviceClient.Execute(req1);
            var resp1Deserialized = JsonSerializer.Deserialize<ContactRolesDto>(resp1.invln_portalroles);

            var req09 = new invln_getorganizationdetailsRequest() //get contact role
            {
                invln_contactexternalid = resp1Deserialized.externalId,
                invln_accountid = resp1Deserialized.contactRoles[0].accountId.ToString(), //account
            };

            var resp09 = (invln_getorganizationdetailsResponse)serviceClient.Execute(req09);
            var resp09Deserialized = JsonSerializer.Deserialize<OrganizationDetailsDto>(resp09.invln_organizationdetails);


            var req10 = new invln_sendinvestmentloansdatatocrmRequest() //create or update loan application and create or update related site details
            {
                invln_contactexternalid = resp1Deserialized.externalId, //contact external id
                //invln_loanapplicationid = resp2Element.loanApplicationId, //loan app id
                invln_accountid = resp1Deserialized.contactRoles[0].accountId.ToString(), //account
                invln_entityfieldsparameters = JsonSerializer.Serialize(new LoanApplicationDto()
                {
                    LoanApplicationContact = new UserAccountDto() { ContactEmail = "abcd@aa.cc", ContactExternalId = resp1Deserialized.externalId, ContactFirstName = "Ala", ContactLastName = "Test", AccountId = Guid.Parse(req09.invln_accountid), ContactTelephoneNumber = "8888" },
                }), //serialized Loan Application DTO
            };

            var resp10 = (invln_sendinvestmentloansdatatocrmResponse)serviceClient.Execute(req10);

            var req11 = new invln_getsingleloanapplicationforaccountandcontactRequest() //get single loan application
            {
                invln_accountid = resp1Deserialized.contactRoles[0].accountId.ToString(), //accountid
                invln_externalcontactid = resp1Deserialized.externalId, //external contact id
                invln_loanapplicationid = resp10.invln_loanapplicationid //loan application id
            };
            var resp11 = (invln_getsingleloanapplicationforaccountandcontactResponse)serviceClient.Execute(req11);
            //Console.WriteLine("Status: " + resp11.invln_loanapplication);
            var req11_1 = new invln_changeloanapplicationexternalstatusRequest()
            {
                invln_loanapplicationid = resp10.invln_loanapplicationid,
                invln_statusexternal = (int)invln_ExternalStatus.ApplicationSubmitted
            };
            var resp11_1 = (invln_changeloanapplicationexternalstatusResponse)serviceClient.Execute(req11_1);

            var req112 = new invln_getsingleloanapplicationforaccountandcontactRequest() //get single loan application
            {
                invln_accountid = resp1Deserialized.contactRoles[0].accountId.ToString(), //accountid
                invln_externalcontactid = resp1Deserialized.externalId, //external contact id
                invln_loanapplicationid = resp10.invln_loanapplicationid //loan application id
            };
            var resp112 = (invln_getsingleloanapplicationforaccountandcontactResponse)serviceClient.Execute(req11);

            var req2 = new invln_getloanapplicationsforaccountandcontactRequest() //get loan applications related to account and contact with given data
            {
                invln_accountid = resp1Deserialized.contactRoles[0].accountId.ToString(), //account id
                invln_externalcontactid = resp1Deserialized.externalId, // contact external id
            };
            var resp2 = (invln_getloanapplicationsforaccountandcontactResponse)serviceClient.Execute(req2);

            var resp2Deserialized = JsonSerializer.Deserialize<List<LoanApplicationDto>>(resp2.invln_loanapplications); //list deserialization
            foreach (var resp2Element in resp2Deserialized) //for every element in deserialized list
            {
                var req3 = new invln_sendinvestmentloansdatatocrmRequest() //create or update loan application and create or update related site details
                {
                    invln_contactexternalid = resp2Element.externalId, //contact external id
                    invln_loanapplicationid = resp2Element.loanApplicationId, //loan app id
                    invln_accountid = resp2Element.accountId.ToString(), //account
                    invln_entityfieldsparameters = JsonSerializer.Serialize(resp2Element), //serialized Loan Application DTO
                };
                var resp3 = (invln_sendinvestmentloansdatatocrmResponse)serviceClient.Execute(req3);

                //var req4 = new invln_changeloanapplicationexternalstatusRequest() //change status
                //{
                //    invln_loanapplicationid = resp2Element.loanApplicationId,
                //    invln_statusexternal = 858110003,
                //};

                //serviceClient.Execute(req4);
            }
        }

    }
}
