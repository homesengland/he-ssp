using AngleSharp.Html.Dom;
using HE.Investments.Common.Extensions;
using HE.Investments.IntegrationTestsFramework.Auth;
using HE.Investments.IntegrationTestsFramework.Data;
using HE.Investments.IntegrationTestsFramework.Exceptions;
using HE.Investments.TestsUtils.Helpers;

namespace HE.Investments.IntegrationTestsFramework;

public class IntegrationTestClient
{
    private readonly HttpClient _client;

    private readonly ILoginData _loginData;

    public IntegrationTestClient(HttpClient client, ILoginData loginData)
    {
        _client = client;
        _client.BaseAddress = new Uri("https://localhost/");
        _loginData = loginData;
        AsLoggedUser();
    }

    public IHtmlDocument CurrentPage { get; private set; }

    public IntegrationTestClient AsLoggedUser()
    {
        ClearAuthHeaders();
        _client.DefaultRequestHeaders.Add(TestAuthHandler.HeaderUserGlobalId, _loginData.UserGlobalId);
        _client.DefaultRequestHeaders.Add(TestAuthHandler.HeaderUserEmail, _loginData.Email);
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
        CurrentPage = await HtmlHelpers.GetDocumentAsync(response);
        return CurrentPage;
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

    public async Task<IHtmlDocument> SubmitButton(
        IHtmlButtonElement submitButton,
        IEnumerable<KeyValuePair<string, string>> formValues,
        IEnumerable<(string InputName, FileEntry File)>? formFiles = null)
    {
        var form = submitButton.Form!;

        foreach (var formValue in formValues)
        {
            var radiosFound = HandleRadioInputs(form, formValue);
            var checkboxesFound = HandleCheckboxInputs(form, formValue);
            var textAreaFound = HandleTextAreaInputs(form, formValue);
            var inputFound = HandleInputs(form, formValue);

            if (!radiosFound && !textAreaFound && !inputFound && !checkboxesFound)
            {
                throw new HtmlElementNotFoundException($"Cannot found any input with name {formValue.Key}");
            }
        }

        if (formFiles != null)
        {
            foreach (var formFile in formFiles)
            {
                if (!HandleFileInputs(form, formFile.InputName, formFile.File))
                {
                    throw new HtmlElementNotFoundException($"Cannot found any file input with name {formFile.InputName}");
                }
            }
        }

        var submit = form.GetSubmission(submitButton)!;
        var target = (Uri)submit.Target;
        using var submission = new HttpRequestMessage(new HttpMethod(submit.Method.ToString()), target);
        submission.Content = new StreamContent(submit.Body);

        foreach (var header in submit.Headers)
        {
            submission.Headers.TryAddWithoutValidation(header.Key, header.Value);
            submission.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        var clientResponse = await _client.SendAsync(submission);
        CurrentPage = await HtmlHelpers.GetDocumentAsync(clientResponse);
        return CurrentPage;
    }

    private static bool HandleRadioInputs(IHtmlFormElement form, KeyValuePair<string, string> formValue)
    {
        var radioInputs = form.Elements
            .Select(x => x as IHtmlInputElement)
            .Where(x => x is not null && x.Type == "radio")
            .ToList();

        var radioInputsWithFormName = radioInputs.Where(radio => radio!.Name.IsProvided() && radio.Name == formValue.Key).ToList();

        if (!radioInputsWithFormName.IsNotEmpty())
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

        var inputElement = radioInputsWithFormName.SingleOrDefault(x => x!.Value == formValue.Value) ??
                           throw new HtmlElementNotFoundException(
                               $"None of radio buttons for property {formValue.Key}, has value {formValue.Value}. Possible values: {string.Join(", ", radioInputsWithFormName.Select(x => x!.Value))}");

        inputElement.IsChecked = true;

        return true;
    }

    private static bool HandleCheckboxInputs(IHtmlFormElement form, KeyValuePair<string, string> formValue)
    {
        var checkboxInputs = form.Elements
            .Select(x => x as IHtmlInputElement)
            .Where(x => x is not null && x.Type == "checkbox")
            .ToList();

        var checkboxInputsWithFormName = checkboxInputs.Where(checkbox => checkbox!.Name.IsProvided() && checkbox.Name!.Contains(formValue.Key)).ToList();

        if (!checkboxInputsWithFormName.IsNotEmpty())
        {
            return false;
        }

        if (formValue.Value == string.Empty)
        {
            foreach (var checkbox in checkboxInputsWithFormName)
            {
                checkbox!.IsChecked = false;
            }

            return true;
        }

        var inputElement = checkboxInputsWithFormName.SingleOrDefault(x => x!.Value == formValue.Value) ??
                           throw new HtmlElementNotFoundException($"None of checkboxes for property {formValue.Key}, has value {formValue.Value}");

        inputElement.IsChecked = true;

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

        inputElement.Value = formValue.Value;

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

        inputElement.Value = formValue.Value;

        return true;
    }

    private static bool HandleFileInputs(IHtmlFormElement form, string inputName, FileEntry file)
    {
        var fileInputs = form.Elements
            .Select(x => x as IHtmlInputElement)
            .Where(x => x is not null && x.Type == "file")
            .ToList();

        var inputElement = fileInputs.SingleOrDefault(x => x!.Name == inputName);
        if (inputElement is null)
        {
            return false;
        }

        inputElement.Files!.Add(file);

        return true;
    }

    private void ClearAuthHeaders()
    {
        _client.DefaultRequestHeaders.Remove(TestAuthHandler.HeaderUserGlobalId);
        _client.DefaultRequestHeaders.Remove(TestAuthHandler.HeaderUserEmail);
    }
}
