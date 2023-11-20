using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

#pragma warning disable CA1716
namespace HE.Investments.Common.WWW.Components.UploadFile;
#pragma warning restore CA1716

public class UploadFile : ViewComponent
{
    public IViewComponentResult Invoke(string fieldName, string description, int? maxFileSizeInMb = null)
    {
        var (hasFileError, fileErrorMessage) = ViewData.ModelState.GetErrors(fieldName);
        return View("UploadFile", (fieldName, maxFileSizeInMb, description, hasFileError, fileErrorMessage));
    }
}
