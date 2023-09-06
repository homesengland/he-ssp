using System.Net.Http.Headers;
using AngleSharp.Html.Dom;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Auth;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Helpers;

namespace HE.InvestmentLoans.IntegrationTests.IntegrationFramework;

public class IntegrationTestClient
{
    private readonly HttpClient _client;

    public IntegrationTestClient(HttpClient client)
    {
        _client = client;
    }

    public IntegrationTestClient AsLoggedUser()
    {
        _client.BaseAddress = new Uri("https://localhost/");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        _client.DefaultRequestHeaders.Add("UserId", "1");
        return this;
    }

    public IntegrationTestClient AsNotLoggedUser()
    {
        _client.BaseAddress = new Uri("https://localhost/");
        _client.DefaultRequestHeaders.Remove("UserId");
        return this;
    }

    public async Task<IHtmlDocument> NavigateTo(string path)
    {
        var response = await _client.GetAsync(path);
        return await HtmlHelpers.GetDocumentAsync(response);
    }

    public async Task<IHtmlDocument> ClickAhref(IHtmlAnchorElement anchorElement)
    {
        return await NavigateTo(anchorElement.PathName);
    }

    public async Task<IHtmlDocument> SubmitButton(IHtmlButtonElement submitButton)
    {
        var form = submitButton.Form!;
        var submit = form.GetSubmission(submitButton)!;
        var target = (Uri)submit.Target;
        using var submission = new HttpRequestMessage(new HttpMethod(submit.Method.ToString()), target)
        {
            Content = new StreamContent(submit.Body),
        };

        foreach (var header in submit.Headers)
        {
            submission.Headers.TryAddWithoutValidation(header.Key, header.Value);
            submission.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        return await HtmlHelpers.GetDocumentAsync(await _client.SendAsync(submission));
    }
}
