using HE.Investments.Common.WWW.Models;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class HomeTypeListModel : HomeTypeModelBase, IEditableViewModel
{
    public HomeTypeListModel(string applicationName)
        : base(applicationName)
    {
    }

    public HomeTypeListModel()
        : base(string.Empty)
    {
    }

    public bool IsEditable { get; set; }

    public bool IsReadOnly => !IsEditable;

    public IList<HomeTypeItemModel> HomeTypes { get; set; }
}
