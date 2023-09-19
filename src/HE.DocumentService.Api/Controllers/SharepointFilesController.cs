using HE.DocumentService.SharePoint.Interfaces;
using HE.DocumentService.SharePoint.Models.File;
using HE.DocumentService.SharePoint.Models.Table;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Office.SharePoint.Tools;

namespace HE.DocumentService.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class SharepointFilesController : CustomControllerBase
{
    public SharepointFilesController(
        IHttpContextAccessor contextAccessor)
        : base(contextAccessor)
    {
    }

    [HttpPost("GetTableRows")]
    public async Task<TableResult<FileTableRow>> GetTableRows(FileTableFilter filter)
    {
        var rows = await Service<ISharePointFilesService>().GetTableRows(Guid.NewGuid(), filter);

        return rows;
    }

    [HttpPost("Upload")]
    public async Task Upload(FileData item)
    {
        await Service<ISharePointFilesService>().UploadFile(Guid.NewGuid(), item);
    }

    [HttpGet("Download")]
    public async Task<FileData> Download(string fileName)
    {
        var file = await Service<ISharePointFilesService>().DownloadFile(Guid.NewGuid(), fileName);

        return file;
    }

    [HttpDelete("Delete")]
    public async Task Delete(string fileName)
    {
        await Service<ISharePointFilesService>().RemoveFile(Guid.NewGuid(), fileName);
    }
}
