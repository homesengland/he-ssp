using HE.Investments.Common.WWW.Enums;

namespace HE.Investments.Common.WWW.Components.Table;

public record TableHeaderViewModel(string Title, CellWidth Width = CellWidth.Undefined, bool IsHidden = false, bool IsDisplayed = true);
