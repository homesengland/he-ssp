using HE.Investments.DocumentService.Models.File;
using HE.Investments.DocumentService.Models.Table;
using HE.Investments.DocumentService.Services;

namespace HE.InvestmentLoans.IntegrationTests.Loans.CompanyStructureSection.Mock;

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
}
