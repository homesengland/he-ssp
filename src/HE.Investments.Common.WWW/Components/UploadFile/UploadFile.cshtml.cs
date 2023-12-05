using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HE.Investments.Common.WWW.Components.UploadFile;

public class UploadFile : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        string? description = null,
        string? allowedExtensions = null,
        bool isMultiple = false,
        bool isHidden = false,
        bool hideFileSize = false,
        int? maxFileSizeInMb = null,
        string? title = null,
        string? uploadOneFileUrl = null,
        string? removeFileUrlTemplate = null,
        string? downloadFileUrlTemplate = null)
    {
        var (hasFileError, fileErrorMessage) = ViewData.ModelState.GetErrors(fieldName);
        var multiple = isMultiple ? "multiple" : string.Empty;
        return View(
            "UploadFile",
            (title, fieldName, allowedExtensions, hideFileSize, maxFileSizeInMb, description, multiple, isHidden, hasFileError, fileErrorMessage, uploadOneFileUrl, removeFileUrlTemplate, downloadFileUrlTemplate));
    }
}
