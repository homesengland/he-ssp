using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.Table;

public class Table : ViewComponent
{
    public IViewComponentResult Invoke(IList<TableHeaderViewModel> headers, IList<TableRowViewModel> rows)
    {
        return View("Table", (headers, rows));
    }
}
