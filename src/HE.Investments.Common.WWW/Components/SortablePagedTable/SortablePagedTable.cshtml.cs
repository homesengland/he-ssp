using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.WWW.Components.Table;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.SortablePagedTable;

public class SortablePagedTable : ViewComponent
{
    public IViewComponentResult Invoke(IList<TableHeaderViewModel> headers, PaginationResult<TableRowViewModel> rows, string navigationUrl)
    {
        return View("SortablePagedTable", (headers, rows, navigationUrl));
    }
}
