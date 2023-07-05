using HE.Xrm.ServiceClientExample.Model;
using HE.Xrm.ServiceClientExample.Model.EntitiesDto;
using Microsoft.Identity.Client;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Rest;
using Microsoft.Xrm.Sdk;
using System;
using System.Configuration;
using System.Text.Json;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

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
        //usun�� secret
        static string Url = "https://investmentsdev.crm11.dynamics.com";
        static string ClientId = "686b9fef-faef-40fa-9195-01f8df059830";
        static string ClientSecret = "Rvb8Q~9VhOe.fC0CY2vETf8GvrsCwnGjCpICbcNV";
        static string connectionString = $@"AuthType = ClientSecret; ClientId={ClientId};Url={Url};ClientSecret={ClientSecret};";

        static void Main(string[] args)
        {
            //var connectionString = "";
            //using (var reader = new StreamReader(Directory.GetCurrentDirectory() + "../../../../appsettings.json"))
            //{
            //    var appSettings = JsonSerializer.Deserialize<AppSettings>(reader.ReadToEnd());
            //    if (appSettings != null)
            //    {
            //        connectionString = $@"AuthType = ClientSecret; ClientId={appSettings.ClientId};Url={appSettings.Url};ClientSecret={appSettings.ClientSecret};";
            //    }
            //    else
            //    {
            //        throw new ConfigurationException("Missing configuration");
            //    }
            //}



            using (ServiceClient serviceClient = new(connectionString))
            {
                if (serviceClient.IsReady)
                {
                    GenerateRichTextCustomApiTest(serviceClient);
                }
                else
                {
                    Console.WriteLine("A web service connection was not established.");
                }
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        private static void GenerateRichTextCustomApiTest(ServiceClient serviceClient)
        {
            var req = new OrganizationRequest("invln_generaterichtextdocument")  //Name of Custom API
            {
                ["invln_entityid"] = "259834c1-351a-ee11-8f6c-6045bd0d7d6d",  //Input Parameter
                ["invln_entityname"] = "invln_loanapplication",  //Input Parameter 
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

        private static void CheckInvestmensDataLoanCustomApi(ServiceClient serviceClient)
        {
            var siteDetails1 = new SiteDetailsDto()
            {
                Name = "name test1",
                currentValue = new Money(222),
                dateOfPurchase = DateTime.Now.AddDays(-2),
                existingLegalCharges = false,
                existingLegalChargesInformation = "nothing",
                numberOfAffordableHomes = 3,
                numberOfHomes = 5,
                siteCost = new Money(33333),
                planningReferenceNumber = "123123123",
            };
            var siteDetails2 = new SiteDetailsDto()
            {
                Name = "name tes2",
                currentValue = new Money(333),
                dateOfPurchase = DateTime.Now.AddDays(2),
                existingLegalCharges = true,
                existingLegalChargesInformation = "yes",
                numberOfAffordableHomes = 6,
                numberOfHomes = 9,
                siteCost = new Money(2222),
                planningReferenceNumber = "997998999",
            };
            var loanapplicaition = new LoanApplicationDto()
            {
                name = "custom api test loan application + site details + contact + account",
                numberOfSites = 12,
                companyExperience = 3,
                companyPurpose = true,
                companyStructureInformation = "info",
                confirmationDirectorLoansCanBeSubordinated = false,
                costsForAdditionalProjects = new Money(30),
                debentureHolder = "holder",
                directorLoads = true,
                existingCompany = true,
                fundingReason = new OptionSetValue(858110000),
                fundingTypeForAdditionalProjects = new OptionSetValue(858110000),
                outstandingLegalChargesOrDebt = true,
                privateSectorApproach = true,
                privateSectorApproachInformation = "information",
                projectAbnormalCosts = true,
                projectAbnormalCostsInformation = "costs information",
                projectEstimatedTotalCost = new Money(300),
                projectGdv = new Money(333),
                reasonForDirectorLoanNotSubordinated = "reason",
                refinanceRepayment = new OptionSetValue(858110000),
                refinanceRepaymentDetails = "refinance payment details",
                id = new Guid("10525f11-c5fb-ed11-8f6d-002248c653e1"),
                siteDetailsList = new List<SiteDetailsDto>() { siteDetails1, siteDetails2 },
                contactEmailAdress = "email@customapi.pl",
                //accountId = new Guid(""),
            };
            Console.WriteLine("call custom api ");// + i);
            string test = JsonSerializer.Serialize<LoanApplicationDto>(loanapplicaition);
            var req = new OrganizationRequest("invln_sendloansinvestmentdatatocrm")  //Name of Custom API
            {
                ["invln_entityparameters"] = test  //Input Parameter
            };

            var resp = serviceClient.Execute(req);
        }
    }
}
