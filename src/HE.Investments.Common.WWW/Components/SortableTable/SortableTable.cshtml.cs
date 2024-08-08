using HE.Investments.Common.WWW.Components.Table;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.SortableTable;

public class SortableTable : ViewComponent
{
    public IViewComponentResult Invoke(IList<TableHeaderViewModel> headers, IList<TableRowViewModel> rows)
    {
        return View("SortableTable", (headers, rows));
    }
}
