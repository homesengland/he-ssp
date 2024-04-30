using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using HE.Investments.Organisation.CompaniesHouse.Contract;
using Microsoft.AspNetCore.WebUtilities;

namespace HE.Investments.Organisation.CompaniesHouse;

internal class CompaniesHouseApi : ICompaniesHouseApi
{
    private const int MaxCompaniesHouseHits = 10000;

    private readonly HttpClient _httpClient;

    public CompaniesHouseApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CompaniesHouseSearchResult> Search(string organisationName, PagingQueryParams? pagingQueryParams, CancellationToken cancellationToken)
    {
        var queryParams = new Dictionary<string, string?>
        {
            ["company_name_includes"] = organisationName,
            ["size"] = pagingQueryParams?.Size.ToString(CultureInfo.InvariantCulture) ?? "50",
            ["start_index"] = pagingQueryParams?.StartIndex.ToString(CultureInfo.InvariantCulture) ?? "0",
        };

        var response = await _httpClient.GetAsync(QueryHelpers.AddQueryString("advanced-search/companies", queryParams), cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new CompaniesHouseSearchResult { Items = [] };
        }

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<CompaniesHouseSearchResult>(cancellationToken: cancellationToken);

        if (result?.Hits > MaxCompaniesHouseHits)
        {
            result.Hits = MaxCompaniesHouseHits;
        }

        return result!;
    }

    public async Task<CompaniesHouseGetByCompanyNumberResult?> GetByCompanyNumber(string companyNumber, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"company/{companyNumber}", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var errorResult = await response.Content.ReadFromJsonAsync<CompaniesHouseErrorsResult>(cancellationToken: cancellationToken);
            if (errorResult?.Errors != null && Array.Exists(errorResult.Errors, x => x.Error == CompaniesHouseErrors.CompanyProfileNotFound))
            {
                return null;
            }
        }

        response.EnsureSuccessStatusCode();
        var successResult = await response.Content.ReadFromJsonAsync<CompaniesHouseGetByCompanyNumberResult>(cancellationToken: cancellationToken);
        return successResult!;
    }
}
