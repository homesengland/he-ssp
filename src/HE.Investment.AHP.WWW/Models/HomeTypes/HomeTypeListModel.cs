namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class HomeTypeListModel : HomeTypeModelBase
{
    public HomeTypeListModel(string applicationName)
        : base(applicationName)
    {
    }

    public HomeTypeListModel()
        : base(string.Empty)
    {
    }

    public IList<HomeTypeItemModel> HomeTypes { get; set; }
}
