namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class HomeTypeListModel : HomeTypeModelBase
{
    public HomeTypeListModel(string schemeName)
        : base(schemeName)
    {
    }

    public HomeTypeListModel()
        : base(string.Empty)
    {
    }

    public IList<HomeTypeItemModel> HomeTypes { get; set; }
}
