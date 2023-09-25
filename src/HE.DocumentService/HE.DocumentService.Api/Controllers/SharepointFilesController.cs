using System.ComponentModel.DataAnnotations;
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
        var rows = await Service<ISharePointFilesService>().GetTableRows(filter);

        return rows;
    }

    [HttpPost("Upload")]
    public async Task Upload([FromForm] SharepointFileUploadModel item)
    {
        await Service<ISharePointFilesService>().UploadFile(item);
    }

    [HttpGet("Download")]
    public async Task<FileData> Download(string listAlias, string folderPath, string fileName)
    {
        var file = await Service<ISharePointFilesService>().DownloadFile(listAlias, folderPath, fileName);

        return file;
    }

    [HttpDelete("Delete")]
    public async Task Delete(string listAlias, string folderPath, string fileName)
    {
        await Service<ISharePointFilesService>().RemoveFile(listAlias, folderPath, fileName);
    }
}
