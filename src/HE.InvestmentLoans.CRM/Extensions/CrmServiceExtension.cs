using Microsoft.Extensions.DependencyInjection;
using Microsoft.PowerPlatform.Dataverse.Client;
using HE.InvestmentLoans.Common.Models.App;
using Microsoft.Xrm.Sdk;

namespace HE.InvestmentLoans.CRM.Extensions
{
    public static class CrmServiceExtension
    {
        public static void AddCrmConnection(this IServiceCollection services)
        {
            services.AddScoped<IOrganizationService> (services =>
            {
                var config = services.GetRequiredService<IDataverseConfig>();
                var connectionString = $@"
                    AuthType=ClientSecret;
                    Url={config.BaseUri};
                    ClientId={config.ClientId};
                    ClientSecret={config.ClientSecret}
            ";

                return new ServiceClient(connectionString);
            });
        }
    }
}
