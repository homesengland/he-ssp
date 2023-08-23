using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Organisation.CompaniesHouse;

public static class AddCompaniesHouseHttpClientExtensions
{
    public static void AddCompaniesHouseHttpClient(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient<ICompaniesHouseApi, CompaniesHouseApi>((provider, client) =>
        {
            var companiesHouseConfig = provider.GetRequiredService<ICompaniesHouseConfig>();
            client.BaseAddress = companiesHouseConfig.CompaniesHouseBaseUrl;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", companiesHouseConfig.ApiKey);
        });
    }
}
