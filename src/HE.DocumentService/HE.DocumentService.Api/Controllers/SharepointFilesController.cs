using HE.DocumentService.SharePoint.Interfaces;
using HE.DocumentService.SharePoint.Models.File;
using HE.DocumentService.SharePoint.Models.Table;
using Microsoft.AspNetCore.Mvc;

namespace HE.DocumentService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SharepointFilesController : ControllerBase
{
    private readonly ISharePointFilesService _sharepointFileService;

    public SharepointFilesController(ISharePointFilesService sharepointFileService)
    {
        _sharepointFileService = sharepointFileService;
    }

    [HttpPost("GetTableRows")]
    public async Task<TableResult<FileTableRow>> GetTableRows(FileTableFilter filter)
    {
        return await _sharepointFileService.GetTableRows(filter);
    }

    [HttpPost("Upload")]
    [DisableRequestSizeLimit]
    public async Task Upload([FromForm] FileUploadModel item)
    {
        await _sharepointFileService.UploadFile(item);
    }

    [HttpGet("Download")]
    public async Task<IActionResult> Download(string listAlias, string folderPath, string fileName)
    {
        var fileStream = await _sharepointFileService.DownloadFileStream(listAlias, folderPath, fileName);
        return File(fileStream, "application/octet-stream", fileName);
    }

    [HttpDelete("Delete")]
    public async Task Delete(string listAlias, string folderPath, string fileName)
    {
        await _sharepointFileService.RemoveFile(listAlias, folderPath, fileName);
    }

    [HttpPost("CreateFolders")]
    public void CreateFolders(string listTitle, List<string> folderPaths)
    {
        _sharepointFileService.CreateFolders(listTitle, folderPaths);
    }
}
