using HE.Investments.Common.Contract;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.Table;

public class Table : ViewComponent
{
    public IViewComponentResult Invoke(IList<TableHeaderViewModel> headers, IList<TableRowViewModel> rows)
    {
        return View("Table", (headers, rows));
    }
}

public record TableHeaderViewModel(string Title, int? Width = null, bool IsHidden = false);

public record TableRowViewModel(IList<TableValueViewModel> Values);

public record TableValueViewModel(string? Value = null, ComponentViewModel? Component = null);

public record ComponentViewModel(string Name, object? Arguments);
