using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HE.Investments.Common.WWW.Components.UploadFile;

public class UploadFile : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        string description,
        bool isMultiple = false,
        bool isHidden = false,
        int? maxFileSizeInMb = null,
        string? title = null,
        string? uploadOneFileUrl = null,
        string? removeFileUrlTemplate = null)
    {
        var (hasFileError, fileErrorMessage) = ViewData.ModelState.GetErrors(fieldName);
        var multiple = isMultiple ? "multiple" : string.Empty;
        return View(
            "UploadFile",
            (title, fieldName, maxFileSizeInMb, description, multiple, isHidden, hasFileError, fileErrorMessage, uploadOneFileUrl, removeFileUrlTemplate));
    }
}
