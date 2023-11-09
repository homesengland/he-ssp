using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using HE.DocumentService.SharePoint.Interfaces;
using HE.DocumentService.SharePoint.Models.File;
using HE.DocumentService.SharePoint.Models.Table;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Microsoft.Office.SharePoint.Tools;
using Microsoft.SharePoint.News.DataModel;
using Newtonsoft.Json;
using PnP.Framework.Modernization.Entities;

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
    [DisableRequestSizeLimit]
    public async Task Upload([FromForm] FileUploadModel item)
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

    [HttpPost("CreateFolders")]
    public void CreateFolders(string listTitle, List<string> folderPaths)
    {
        Service<ISharePointFilesService>().CreateFolders(listTitle, folderPaths);
    }
}
