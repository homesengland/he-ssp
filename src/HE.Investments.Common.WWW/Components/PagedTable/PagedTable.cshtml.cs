using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.WWW.Components.Table;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.PagedTable;

public class PagedTable : ViewComponent
{
    public IViewComponentResult Invoke(IList<TableHeaderViewModel> headers, PaginationResult<TableRowViewModel> rows, string navigationUrl)
    {
        return View("PagedTable", (headers, rows, navigationUrl));
    }
}
