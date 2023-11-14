using System.Text;
using System.Text.Json;
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

    private readonly ILogger<HttpDocumentService> _logger;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        MaxDepth = 64,
        PropertyNameCaseInsensitive = true,
    };

    public HttpDocumentService(IHttpClientFactory httpClient, IDocumentServiceConfig config, ILogger<HttpDocumentService> logger)
    {
        _httpClient = httpClient;
        _config = config;
        _logger = logger;
    }

    public async Task<TableResult<FileTableRow>> GetTableRowsAsync(FileTableFilter filter)
    {
        var uri = new UriBuilder($"{_config.Url}{"/SharepointFiles/GetTableRows"}");
        using var request = new HttpRequestMessage(HttpMethod.Post, uri.ToString())
        {
            Content = new StringContent(JsonSerializer.Serialize(filter), Encoding.UTF8, "application/json"),
        };

        return await SendAsync<TableResult<FileTableRow>>(request);
    }

    public async Task UploadAsync(FileUploadModel item)
    {
        var uri = new UriBuilder($"{_config.Url}{"/SharepointFiles/Upload"}");

        using var listTitleStringContent = new StringContent(item.ListTitle);
        using var folderPathStringContent = new StringContent(item.FolderPath);
        using var metadataStringContent = new StringContent(item.Metadata);
        using var overwriteStringContent = new StringContent(item.Overwrite.HasValue ? item.Overwrite.Value.ToString() : string.Empty);

        using var multipartContent = new MultipartFormDataContent
        {
            { listTitleStringContent, "ListTitle" },
            { folderPathStringContent, "FolderPath" },
            { metadataStringContent, "Metadata" },
            { overwriteStringContent, "Overwrite" },
        };

        using var fileContent = new StreamContent(item.File.OpenReadStream());
        multipartContent.Add(fileContent, "File", item.File.FileName);

        using var request = new HttpRequestMessage(HttpMethod.Post, uri.ToString())
        {
            Content = multipartContent,
        };
        using var response = await SendAsync(request);
    }

    public async Task DeleteAsync(string listAlias, string folderPath, string fileName)
    {
        var queryParams = $"listAlias={listAlias}&folderPath={folderPath}&fileName={fileName}";
        var uri = new UriBuilder($"{_config.Url}{"/SharepointFiles/Delete"}?{queryParams}");
        using var request = new HttpRequestMessage(HttpMethod.Delete, uri.ToString());
        using var response = await SendAsync(request);
    }

    public async Task<FileData> DownloadAsync(string listAlias, string folderPath, string fileName)
    {
        var queryParams = $"listAlias={listAlias}&folderPath={folderPath}&fileName={fileName}";
        var uri = new UriBuilder($"{_config.Url}{"/SharepointFiles/Download"}?{queryParams}");
        using var request = new HttpRequestMessage(HttpMethod.Get, uri.ToString());
        using var response = await SendAsync(request);

        var fileStream = await response.Content.ReadAsStreamAsync();
        var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        return new FileData(fileName, memoryStream);
    }

    public async Task CreateFoldersAsync(string listTitle, List<string> folderPaths)
    {
        var queryParams = $"listTitle={listTitle}";
        var uri = new UriBuilder($"{_config.Url}{"/SharepointFiles/CreateFolders"}?{queryParams}");

        using var request = new HttpRequestMessage(HttpMethod.Post, uri.ToString())
        {
            Content = new StringContent(JsonSerializer.Serialize(folderPaths), Encoding.UTF8, "application/json"),
        };
        using var response = await SendAsync(request);
    }

    private async Task<TResponse> SendAsync<TResponse>(HttpRequestMessage request)
    {
        using var response = await SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        try
        {
            var serializedResponse = JsonSerializer.Deserialize<TResponse>(responseContent, _jsonSerializerOptions);
            if (serializedResponse != null)
            {
                return serializedResponse;
            }

            _logger.LogError("Document Service response for {Method} {Url} is null.", request.Method, request.RequestUri);
            throw new DocumentServiceSerializationException(responseContent);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Document Service response for {Method} {Url} cannot be deserialized.", request.Method, request.RequestUri);
            throw new DocumentServiceSerializationException(responseContent, ex);
        }
    }

    private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        using var client = _httpClient.CreateClient("HE.Investments.DocumentService");
        client.Timeout = TimeSpan.FromMinutes(3);

        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError(
                "Document Service returned {StatusCode} for {Method} {Url} with content \"{Content}\".",
                response.StatusCode,
                request.Method,
                request.RequestUri,
                errorContent);

            throw new DocumentServiceCommunicationException(request.Method, request.RequestUri, response.StatusCode, errorContent);
        }

        return response;
    }
}
