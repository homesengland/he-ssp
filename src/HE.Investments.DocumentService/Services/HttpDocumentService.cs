using System.Text;
using System.Text.Json;
using HE.Investments.DocumentService.Configs;
using HE.Investments.DocumentService.Exceptions;
using HE.Investments.DocumentService.Models;
using HE.Investments.DocumentService.Services.Contract;
using Microsoft.Extensions.Logging;

namespace HE.Investments.DocumentService.Services;

public class HttpDocumentService : IDocumentService
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

    public async Task<IEnumerable<FileDetails<TMetadata>>> GetFilesAsync<TMetadata>(GetFilesQuery query, CancellationToken cancellationToken)
        where TMetadata : class
    {
        var uri = new UriBuilder($"{_config.Url}/SharepointFiles/GetTableRows");
        var content = new FileTableFilterContract
        {
            ListTitle = query.ListTitle,
            ListAlias = query.ListAlias,
            FolderPaths = query.FolderPaths.ToList(),
            PagingInfo = null,
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, uri.ToString())
        {
            Content = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json"),
        };

        var result = await SendAsync<TableResultContract>(request, cancellationToken);
        return result.Items.Select(x => new FileDetails<TMetadata>(
            x.Id,
            x.FileName,
            x.FolderPath,
            x.Size,
            x.Editor,
            x.Modified,
            string.IsNullOrEmpty(x.Metadata) ? null : JsonSerializer.Deserialize<TMetadata>(x.Metadata, _jsonSerializerOptions)));
    }

    public async Task UploadAsync<TMetadata>(FileLocation location, UploadFileData<TMetadata> file, bool overwrite = false, CancellationToken cancellationToken = default)
    {
        var uri = new UriBuilder($"{_config.Url}/SharepointFiles/Upload");

        using var listTitleStringContent = new StringContent(location.ListTitle);
        using var folderPathStringContent = new StringContent(location.FolderPath);
        using var metadataStringContent = new StringContent(JsonSerializer.Serialize(file.Metadata, _jsonSerializerOptions));
        using var overwriteStringContent = new StringContent(overwrite.ToString());

        using var multipartContent = new MultipartFormDataContent
        {
            { listTitleStringContent, "ListTitle" },
            { folderPathStringContent, "FolderPath" },
            { metadataStringContent, "Metadata" },
            { overwriteStringContent, "Overwrite" },
        };

        using var fileContent = new StreamContent(file.Content);
        multipartContent.Add(fileContent, "File", file.Name);

        using var request = new HttpRequestMessage(HttpMethod.Post, uri.ToString())
        {
            Content = multipartContent,
        };
        using var response = await SendAsync(request, cancellationToken);
    }

    public async Task DeleteAsync(FileLocation location, string fileName, CancellationToken cancellationToken)
    {
        var uri = new UriBuilder($"{_config.Url}/SharepointFiles/Delete?listAlias={location.ListAlias}&folderPath={location.FolderPath}&fileName={fileName}");
        using var request = new HttpRequestMessage(HttpMethod.Delete, uri.ToString());
        using var response = await SendAsync(request, cancellationToken);
    }

    public async Task<DownloadFileData> DownloadAsync(FileLocation location, string fileName, CancellationToken cancellationToken)
    {
        var uri = new UriBuilder($"{_config.Url}/SharepointFiles/Download?listAlias={location.ListAlias}&folderPath={location.FolderPath}&fileName={fileName}");
        using var request = new HttpRequestMessage(HttpMethod.Get, uri.ToString());
        using var response = await SendAsync(request, cancellationToken);

        await using var fileStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;

        return new DownloadFileData(fileName, memoryStream);
    }

    public async Task CreateFoldersAsync(string listTitle, List<string> folderPaths, CancellationToken cancellationToken)
    {
        var uri = new UriBuilder($"{_config.Url}/SharepointFiles/CreateFolders?listTitle={listTitle}");
        using var request = new HttpRequestMessage(HttpMethod.Post, uri.ToString())
        {
            Content = new StringContent(JsonSerializer.Serialize(folderPaths), Encoding.UTF8, "application/json"),
        };

        using var response = await SendAsync(request, cancellationToken);
    }

    private async Task<TResponse> SendAsync<TResponse>(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        using var response = await SendAsync(request, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
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

    private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        using var client = _httpClient.CreateClient("HE.Investments.DocumentService");
        client.Timeout = TimeSpan.FromMinutes(3);

        var response = await client.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
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
