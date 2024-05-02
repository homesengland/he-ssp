using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HE.Investments.Common.WWW.Components.FilesTable;

public class FilesTable : ViewComponent
{
    public IViewComponentResult Invoke(ModelExpression aspFor, bool isReadOnly = false)
    {
        return View("FilesTable", (aspFor.Name, aspFor.Model as IList<FileModel> ?? [], isReadOnly));
    }
}
