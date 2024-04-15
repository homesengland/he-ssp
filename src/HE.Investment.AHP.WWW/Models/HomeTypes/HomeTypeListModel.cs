using HE.Investment.AHP.Contract.Application;
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

    public AhpApplicationOperation[] AllowedOperations { get; set; }

    public bool IsEditable => AllowedOperations.Contains(AhpApplicationOperation.Modification);

    public bool IsReadOnly => !IsEditable;

    public int TotalExpectedNumberOfHomes { get; set; }

    public IList<HomeTypeItemModel> HomeTypes { get; set; }
}
