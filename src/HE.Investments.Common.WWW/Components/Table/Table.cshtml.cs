using HE.Investments.Common.Contract;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.Table;

public enum CellWidth
{
    Undefined,
    OneHalf,
    OneThird,
    OneQuarter,
    OneFifth,
    OneSixth,
    OneEighth,
}

public class Table : ViewComponent
{
    public IViewComponentResult Invoke(IList<TableHeaderViewModel> headers, IList<TableRowViewModel> rows)
    {
        return View("Table", (headers, rows));
    }
}

public record TableHeaderViewModel(string Title, CellWidth Width = CellWidth.Undefined, bool IsHidden = false, bool IsDisplayed = true);

public record TableRowViewModel(IList<TableValueViewModel> Values);

public record TableValueViewModel(string? Value = null, DynamicComponentViewModel? Component = null, bool IsDisplayed = true);
