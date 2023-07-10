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
                   
                }
                else
                {
                    Console.WriteLine("A web service connection was not established.");
                }
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
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

    }
}
