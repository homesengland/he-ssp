using AngleSharp.Html.Dom;
using FluentAssertions;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Radios;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.IntegrationTests.Config;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Auth;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Exceptions;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Helpers;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Helpers.DataPackages;
using Microsoft.CodeAnalysis.Operations;

namespace HE.InvestmentLoans.IntegrationTests.IntegrationFramework;

public class IntegrationTestClient
{
    private readonly HttpClient _client;

    private readonly IntegrationUserData _userData;

    public IntegrationTestClient(HttpClient client, IntegrationUserData userData)
    {
        _client = client;
        _client.BaseAddress = new Uri("https://localhost/");
        _userData = userData;
        AsLoggedUser();
    }

    public IntegrationTestClient AsLoggedUser(UserGlobalIdWithEmail? user = null)
    {
        ClearAuthHeaders();
        _client.DefaultRequestHeaders.Add(TestAuthHandler.HeaderUserGlobalId, user?.UserGlobalId ?? _userData.UserGlobalId);
        _client.DefaultRequestHeaders.Add(TestAuthHandler.HeaderUserEmail, user?.Email ?? _userData.Email);
        return this;
    }

    public IntegrationTestClient AsNotLoggedUser()
    {
        ClearAuthHeaders();

        return this;
    }

    public async Task<IHtmlDocument> NavigateTo(string path)
    {
        var response = await _client.GetAsync(path);
        return await HtmlHelpers.GetDocumentAsync(response);
    }

    public async Task<IHtmlDocument> NavigateTo(IHtmlAnchorElement anchorElement)
    {
        var href = anchorElement.GetAttribute("href");
        return await NavigateTo(href!);
    }

    public async Task<IHtmlDocument> SubmitButton(IHtmlButtonElement submitButton)
    {
        return await SubmitButton(submitButton, new Dictionary<string, string>());
    }

    public Task<IHtmlDocument> SubmitButton(IHtmlButtonElement submitButton, params (string InputName, string Value)[] inputs)
    {
        return SubmitButton(submitButton, inputs.Select(i => new KeyValuePair<string, string>(i.InputName, i.Value)));
    }

    public async Task<IHtmlDocument> SubmitButton(IHtmlButtonElement submitButton, IEnumerable<KeyValuePair<string, string>> formValues)
    {
        var form = submitButton.Form!;

        foreach (var formValue in formValues)
        {
            var radiosFound = HandleRadioInputs(form, formValue);
            var textAreaFound = HandleTextAreaInputs(form, formValue);
            var inputFound = HandleInputs(form, formValue);

            if (!radiosFound && !textAreaFound && !inputFound)
            {
                throw new HtmlElementNotFoundException($"Cannot found any input with name {formValue.Key}");
            }
        }

        var submit = form.GetSubmission(submitButton)!;
        var target = (Uri)submit.Target;
        using var submission = new HttpRequestMessage(new HttpMethod(submit.Method.ToString()), target) { Content = new StreamContent(submit.Body), };

        foreach (var header in submit.Headers)
        {
            submission.Headers.TryAddWithoutValidation(header.Key, header.Value);
            submission.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        var clientResponse = await _client.SendAsync(submission);
        return await HtmlHelpers.GetDocumentAsync(clientResponse);
    }

    private static bool HandleRadioInputs(IHtmlFormElement form, KeyValuePair<string, string> formValue)
    {
        var radioInputs = form.Elements
            .Select(x => x as IHtmlInputElement)
            .Where(x => x is not null && x.Type == "radio")
            .ToList();

        var radioInputsWithFormName = radioInputs.Where(radio => radio!.Name.IsProvided() && radio.Name!.Contains(formValue.Key)).ToList();

        if (!radioInputsWithFormName.Any())
        {
            return false;
        }

        foreach (var radioInput in radioInputsWithFormName)
        {
            radioInput!.IsChecked = false;
        }

        if (formValue.Value == string.Empty)
        {
            return true;
        }

        var inputElement = radioInputsWithFormName.SingleOrDefault(x => x!.Value == formValue.Value) ?? throw new HtmlElementNotFoundException($"None of radio buttons for property {formValue.Key}, has value {formValue.Value}");

        inputElement!.IsChecked = true;

        return true;
    }

    private static bool HandleTextAreaInputs(IHtmlFormElement form, KeyValuePair<string, string> formValue)
    {
        var textInputs = form.Elements
            .Select(x => x as IHtmlTextAreaElement)
            .Where(x => x is not null)
            .ToList();

        var inputElement = textInputs.SingleOrDefault(x => x!.Name == formValue.Key);

        if (inputElement is null)
        {
            return false;
        }

        inputElement!.Value = formValue.Value;

        return true;
    }

    private static bool HandleInputs(IHtmlFormElement form, KeyValuePair<string, string> formValue)
    {
        var textInputs = form.Elements
            .Select(x => x as IHtmlInputElement)
            .Where(x => x is not null && x.Type == "text")
            .ToList();

        var inputElement = textInputs.SingleOrDefault(x => x!.Name == formValue.Key);

        if (inputElement is null)
        {
            return false;
        }

        inputElement!.Value = formValue.Value;

        return true;
    }

    private void ClearAuthHeaders()
    {
        _client.DefaultRequestHeaders.Remove(TestAuthHandler.HeaderUserGlobalId);
        _client.DefaultRequestHeaders.Remove(TestAuthHandler.HeaderUserEmail);
    }
}
