using System.Net.Http.Headers;
using System.Text;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.InvestmentLoans.IntegrationTests.Config;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Auth;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Helpers;

namespace HE.InvestmentLoans.IntegrationTests.IntegrationFramework;

public class IntegrationTestClient
{
    private readonly HttpClient _client;

    private readonly IntegrationTestConfig _integrationTestConfig;

    public IntegrationTestClient(HttpClient client, IntegrationTestConfig integrationTestConfig)
    {
        _client = client;
        _client.BaseAddress = new Uri("https://localhost/");
        _integrationTestConfig = integrationTestConfig;
        AsLoggedUser();
    }

    public IntegrationTestClient AsLoggedUser()
    {
        _client.DefaultRequestHeaders.Add(TestAuthHandler.HeaderUserGlobalId, _integrationTestConfig.User.UserGlobalId);
        _client.DefaultRequestHeaders.Add(TestAuthHandler.HeaderUserEmail, _integrationTestConfig.User.Email);

        return this;
    }

    public IntegrationTestClient AsNotLoggedUser()
    {
        _client.DefaultRequestHeaders.Remove(TestAuthHandler.HeaderUserGlobalId);
        return this;
    }

    public async Task<IHtmlDocument> NavigateTo(string path)
    {
        var response = await _client.GetAsync(path);
        return await HtmlHelpers.GetDocumentAsync(response);
    }

    public async Task<IHtmlDocument> ClickAHrefElement(IHtmlAnchorElement anchorElement)
    {
        return await NavigateTo(anchorElement.PathName);
    }

    public async Task<IHtmlDocument> SubmitButton(IHtmlButtonElement submitButton)
    {
        return await SubmitButton(submitButton, new Dictionary<string, string>());
    }

    public async Task<IHtmlDocument> SubmitButton(IHtmlButtonElement submitButton, IEnumerable<KeyValuePair<string, string>> formValues)
    {
        var form = submitButton.Form!;

        foreach (var formValue in formValues)
        {
            HandleRadioInputs(form, formValue);
        }

        var submit = form.GetSubmission(submitButton)!;
        var target = (Uri)submit.Target;
        using var submission = new HttpRequestMessage(new HttpMethod(submit.Method.ToString()), target) { Content = new StreamContent(submit.Body), };

        foreach (var header in submit.Headers)
        {
            submission.Headers.TryAddWithoutValidation(header.Key, header.Value);
            submission.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        return await HtmlHelpers.GetDocumentAsync(await _client.SendAsync(submission));
    }

    private static void HandleRadioInputs(IHtmlFormElement form, KeyValuePair<string, string> formValue)
    {
        var radioInputs = form.Elements
            .Select(x => x as IHtmlInputElement)
            .Where(x => x is not null && x.Type == "radio")
            .ToList();

        if (radioInputs.Count <= 0)
        {
            return;
        }

        var inputElement = radioInputs.SingleOrDefault(x => x!.Value == formValue.Value);
        inputElement.Should().NotBeNull($"{formValue.Key} Key should be radio input element of form");
        inputElement!.IsChecked = true;
    }
}
