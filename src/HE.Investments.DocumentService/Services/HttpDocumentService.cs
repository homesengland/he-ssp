using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HE.Investments.DocumentService.Configs;
using HE.Investments.DocumentService.Exceptions;
using HE.Investments.DocumentService.Models.File;
using HE.Investments.DocumentService.Models.Table;
using Microsoft.Extensions.Logging;

namespace HE.Investments.DocumentService.Services;

public class HttpDocumentService : IHttpDocumentService
{
    private readonly IHttpClientFactory _httpClient;

    private readonly IDocumentServiceConfig _config;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        MaxDepth = 64,
        PropertyNameCaseInsensitive = true
    };

    public HttpDocumentService(
        IHttpClientFactory httpClient,
        IDocumentServiceConfig config
        )
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<TableResult<FileTableRow>> GetTableRowsAsync(FileTableFilter filter)
    {
        var uri = new UriBuilder($"{_config.Url}{"/SharepointFiles/GetTableRows"}");
        using var request = new HttpRequestMessage(HttpMethod.Post, uri.ToString())
        {
            Content = new StringContent(JsonSerializer.Serialize(filter), Encoding.UTF8, "application/json")
        };

        var response = await SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new DocumentServiceException($"There was a problem with the document service connection");
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<TableResult<FileTableRow>>(responseContent, _jsonSerializerOptions) ?? throw new DocumentServiceException($"The document service request result is invalid");
    }

    public async Task UploadAsync(FileUploadModel item)
    {
        var uri = new UriBuilder($"{_config.Url}{"/SharepointFiles/Upload"}");

        using var request = new HttpRequestMessage(HttpMethod.Post, uri.ToString())
        {
            Content = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, "application/json")
        };

        var response = await SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new DocumentServiceException($"There was a problem with the document service connection");
        }
    }

    public async Task DeleteAsync(string listAlias, string folderPath, string fileName)
    {
        var queryParams = $"listAlias={listAlias}&folderPath={folderPath}&fileName={fileName}";
        var uri = new UriBuilder($"{_config.Url}{"/SharepointFiles/Delete"}?{queryParams}");
        using var request = new HttpRequestMessage(HttpMethod.Delete, uri.ToString());

        var response = await SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new DocumentServiceException($"There was a problem with the document service connection");
        }
    }

    public async Task<FileData> DownloadAsync(string listAlias, string folderPath, string fileName)
    {
        var queryParams = $"listAlias={listAlias}&folderPath={folderPath}&fileName={fileName}";
        var uri = new UriBuilder($"{_config.Url}{"/SharepointFiles/Download"}?{queryParams}");
        using var request = new HttpRequestMessage(HttpMethod.Get, uri.ToString());

        var response = await SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new DocumentServiceException($"There was a problem with the document service connection");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<FileData>(responseContent, _jsonSerializerOptions) ?? throw new DocumentServiceException($"The document service request result is invalid");
    }

    private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        using var client = _httpClient.CreateClient("HE.Investments.DocumentService");

        return await client.SendAsync(request);
    }
}
