using System.Net.Http.Json;
using HE.Investments.Organisation.CompaniesHouse.Contract;
using Microsoft.AspNetCore.WebUtilities;

namespace HE.Investments.Organisation.CompaniesHouse;

public class CompaniesHouseApi : ICompaniesHouseApi
{
    private readonly HttpClient _httpClient;

    public CompaniesHouseApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CompaniesHouseSearchResult> Search(string organisationName, CancellationToken cancellationToken)
    {
        var queryParams = new Dictionary<string, string?>
        {
            ["q"] = organisationName,
            ["items_per_page"] = "50",
            ["start_index"] = "0",
        };

        var response = await _httpClient.GetAsync(QueryHelpers.AddQueryString("search/companies", queryParams), cancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<CompaniesHouseSearchResult>(cancellationToken: cancellationToken);

        return result!;
    }
}
