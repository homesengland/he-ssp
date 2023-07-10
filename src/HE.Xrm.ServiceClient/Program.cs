using HE.Xrm.ServiceClientExample.Model;
using HE.Xrm.ServiceClientExample.Model.EntitiesDto;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using System.Configuration;
using System.Text.Json;
using System.Text.RegularExpressions;

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
                    connectionString = $@"AuthType = ClientSecret; ClientId={appSettings.ClientId};Url={appSettings.Url};ClientSecret={appSettings.ClientSecret};";
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
                    TestDuringCall(serviceClient);
                }
                else
                {
                    Console.WriteLine("A web service connection was not established.");
                }
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        private static void TestDuringCall(ServiceClient serviceClient)
        {
            var req = new invln_getcontactroleRequest()
            {
                invln_contactemail = "",
                invln_contactexternalid = "auth0|64a28c7fb67ed30b288d6ff7",
                invln_portaltype = "858110001"
            };

            var resp = (invln_getcontactroleResponse)serviceClient.Execute(req);

            var contractRoleDeserialized = JsonSerializer.Deserialize<ContactRolesDto>(resp.invln_portalroles);

            var payload = "{\"companyPurpose\":\"Yes\",\"existingCompany\":\"132\",\"companyExperience\":123,\"projectGdv\":\"123\",\"projectEstimatedTotalCost\":\"123\",\"projectAbnormalCosts\":\"Yes\",\"projectAbnormalCostsInformation\":\"1234\",\"privateSectorApproach\":\"Yes\",\"privateSectorApproachInformation\":\"1234\",\"additionalProjects\":\"Yes\",\"refinanceRepayment\":\"repay\",\"refinanceRepaymentDetails\":null,\"outstandingLegalChargesOrDebt\":\"Yes\",\"debentureHolder\":\"124\",\"directorLoans\":\"Yes\",\"confirmationDirectorLoansCanBeSubordinated\":\"Yes\",\"reasonForDirectorLoanNotSubordinated\":null,\"siteDetailsList\":[],\"id\":null,\"name\":\"ABC Developments\",\"numberOfSites\":null,\"companyStructureInformation\":null,\"costsForAdditionalProjects\":null,\"fundingReason\":\"Buildingnewhomes\",\"fundingTypeForAdditionalProjects\":null,\"contactEmailAdress\":\"example@mail.com\",\"accountId\":\"429d11ab-15fe-ed11-8f6c-002248c653e1\",\"externalId\":\"auth0|64a28c7fb67ed30b288d6ff7\"}";

            //var req2 = new invln_getsingleloanapplicationforaccountandcontactRequest()
            //{
            //    invln_accountid = contractRoleDeserialized.contactRoles[0].accountId.ToString(),
            //    invln_externalcontactid = req.invln_contactexternalid,
            //    invln_loanapplicationid = "9ecd7050-3c1c-ee11-8f6d-002248c653e1",
            //};
            //var resp2 = (invln_getsingleloanapplicationforaccountandcontactResponse)serviceClient.Execute(req2);

            var req2 = new invln_getloanapplicationsforaccountandcontactRequest()
            {
                invln_externalcontactid = req.invln_contactexternalid,
                invln_accountid = contractRoleDeserialized.contactRoles[0].accountId.ToString(),
            };
            var resp2 = (invln_getloanapplicationsforaccountandcontactResponse)serviceClient.Execute(req2);

            var req2Deserialized = JsonSerializer.Deserialize<List<LoanApplicationDto>>(resp2.invln_loanapplications);

            foreach(var req2Element in req2Deserialized)
            {
                var req3 = new invln_sendinvestmentloansdatatocrmRequest()
                {
                    invln_contactexternalid = req.invln_contactexternalid,
                    invln_loanapplicationid = "9ecd7050-3c1c-ee11-8f6d-002248c653e1",
                    invln_accountid = "429d11ab-15fe-ed11-8f6c-002248c653e1",
                    //invln_entityfieldsparameters = payload,
                    invln_entityfieldsparameters = JsonSerializer.Serialize(req2Element)
                };

                var resp3 = (invln_sendinvestmentloansdatatocrmResponse)serviceClient.Execute(req3);

                Console.WriteLine("loop");
            }
            
            Console.WriteLine("Press any key to exit.");
        }

        private static void GetLoanFromContactAndAccountCustomApi(ServiceClient serviceClient)
        {
            var req = new OrganizationRequest("invln_getsingleloanapplicationforaccountandcontact")  //Name of Custom API
            {
                ["invln_accountid"] = "429d11ab-15fe-ed11-8f6c-002248c653e1",  //Input Parameter
                ["invln_externalcontactid"] = "auth0|64a28c7fb67ed30b288d6ff7",  //Input Parameter
                //["invln_externalcontactid"] = "auth0|64a28c7fb67ed30b288d6ff7",  //Input Parameter
                ["invln_loanapplicationid"] = "njkkjk" //Input Parameter
            };

            var resp = serviceClient.Execute(req);
            Console.WriteLine();
        }

        private static void GenerateRichTextCustomApiTest(ServiceClient serviceClient)
        {
            var req = new OrganizationRequest("invln_generaterichtextdocument")  //Name of Custom API
            {
                ["invln_entityid"] = "9ecd7050-3c1c-ee11-8f6d-002248c653e1",  //Input Parameter
                ["invln_entityname"] = invln_Loanapplication.EntityLogicalName,  //Input Parameter 
                ["invln_richtext"] = "witam {{invln_name}}, jak tam mija zycie? mi dobrze {{invln_companyexperience}}"  //Input Parameter
            };
            string text = "witam {{invln_name}}, jak tam mija zycie? mi dobrze {{invln_companyexperience}}";
            foreach (Match match in Regex.Matches(text, "{[^}]+}"))
            {
                Console.WriteLine(match.Value.Trim('{', '}'));
            }

            var resp = serviceClient.Execute(req);
            Console.WriteLine();
        }

        private static void GetContactRoleCustomApi(ServiceClient serviceClient)
        {
            var req = new OrganizationRequest("invln_getcontactrole")  //Name of Custom API
            {
                ["invln_email"] = "testcustomapi@test.pl",  //Input Parameter
                //["invln_portalid"] = "c5a66e4a-d314-ee11-9cbe-002248c653e1",  //Input Parameter - loans
                ["invln_portalid"] = "0b617644-d314-ee11-9cbe-002248c653e1",  //Input Parameter - AHP
                ["invln_ssid"] = "73972af3-3b06-4caa-b254-599ae93801b5"  //Input Parameter
            };

            var resp = serviceClient.Execute(req);
            Console.WriteLine();
        }

        //private static void CheckInvestmensDataLoanCustomApi(ServiceClient serviceClient)
        //{
        //    var siteDetails1 = new SiteDetailsDto()
        //    {
        //        Name = "name test1",
        //        currentValue = new Money(222),
        //        dateOfPurchase = DateTime.Now.AddDays(-2),
        //        existingLegalCharges = false,
        //        existingLegalChargesInformation = "nothing",
        //        numberOfAffordableHomes = 3,
        //        numberOfHomes = 5,
        //        siteCost = new Money(33333),
        //        planningReferenceNumber = "123123123",
        //    };
        //    var siteDetails2 = new SiteDetailsDto()
        //    {
        //        Name = "name tes2",
        //        currentValue = new Money(333),
        //        dateOfPurchase = DateTime.Now.AddDays(2),
        //        existingLegalCharges = true,
        //        existingLegalChargesInformation = "yes",
        //        numberOfAffordableHomes = 6,
        //        numberOfHomes = 9,
        //        siteCost = new Money(2222),
        //        planningReferenceNumber = "997998999",
        //    };
        //    var loanapplicaition = new LoanApplicationDto()
        //    {
        //        name = "custom api test loan application + site details + contact + account",
        //        numberOfSites = 12,
        //        companyExperience = 3,
        //        companyPurpose = true,
        //        companyStructureInformation = "info",
        //        confirmationDirectorLoansCanBeSubordinated = false,
        //        costsForAdditionalProjects = new Money(30),
        //        debentureHolder = "holder",
        //        directorLoads = true,
        //        existingCompany = true,
        //        fundingReason = new OptionSetValue(858110000),
        //        fundingTypeForAdditionalProjects = new OptionSetValue(858110000),
        //        outstandingLegalChargesOrDebt = true,
        //        privateSectorApproach = true,
        //        privateSectorApproachInformation = "information",
        //        projectAbnormalCosts = true,
        //        projectAbnormalCostsInformation = "costs information",
        //        projectEstimatedTotalCost = new Money(300),
        //        projectGdv = new Money(333),
        //        reasonForDirectorLoanNotSubordinated = "reason",
        //        refinanceRepayment = new OptionSetValue(858110000),
        //        refinanceRepaymentDetails = "refinance payment details",
        //        id = new Guid("10525f11-c5fb-ed11-8f6d-002248c653e1"),
        //        siteDetailsList = new List<SiteDetailsDto>() { siteDetails1, siteDetails2 },
        //        contactEmailAdress = "email@customapi.pl",
        //        //accountId = new Guid(""),
        //    };
        //    Console.WriteLine("call custom api ");// + i);
        //    string test = JsonSerializer.Serialize<LoanApplicationDto>(loanapplicaition);
        //    var req = new OrganizationRequest("invln_sendloansinvestmentdatatocrm")  //Name of Custom API
        //    {
        //        ["invln_entityparameters"] = test  //Input Parameter
        //    };

        //    var resp = serviceClient.Execute(req);
        //}
    }
}
