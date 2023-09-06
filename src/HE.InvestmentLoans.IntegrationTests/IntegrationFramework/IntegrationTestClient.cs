using AngleSharp.Html.Dom;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Helpers;

namespace HE.InvestmentLoans.IntegrationTests.IntegrationFramework;

public class IntegrationTestClient
{
    private readonly HttpClient _client;

    public IntegrationTestClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<IHtmlDocument> NavigateTo(string path)
    {
        var response = await _client.GetAsync(path);
        return await HtmlHelpers.GetDocumentAsync(response);
    }
}
