using HE.Investments.DocumentService.Models.File;
using HE.Investments.DocumentService.Models.Table;
using HE.Investments.DocumentService.Services;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.CompanyStructureSection.Mock;

public class DocumentServiceMock : IHttpDocumentService
{
    public async Task<TableResult<FileTableRow>> GetTableRowsAsync(FileTableFilter filter)
    {
        return await Task.FromResult(new TableResult<FileTableRow>()
        {
            Items = Array.Empty<FileTableRow>(),
            TotalCount = 0,
        });
    }

    public Task UploadAsync(FileUploadModel item)
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string listAlias, string folderPath, string fileName)
    {
        return Task.CompletedTask;
    }

    public Task<FileData> DownloadAsync(string listAlias, string folderPath, string fileName)
    {
        throw new NotImplementedException();
    }

    public Task CreateFoldersAsync(string listTitle, List<string> folderPaths)
    {
        return Task.CompletedTask;
    }
}
