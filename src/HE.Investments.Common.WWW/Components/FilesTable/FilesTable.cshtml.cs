using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

#pragma warning disable CA1716
namespace HE.Investments.Common.WWW.Components.FilesTable;
#pragma warning restore CA1716

public class FilesTable : ViewComponent
{
    public IViewComponentResult Invoke(ModelExpression aspFor)
    {
        return View("FilesTable", (aspFor.Name, aspFor.Model as IList<FileModel>));
    }
}
