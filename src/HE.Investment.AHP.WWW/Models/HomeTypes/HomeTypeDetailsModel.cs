using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class HomeTypeDetailsModel : HomeTypeModelBase
{
    public HomeTypeDetailsModel(string applicationName)
        : base(applicationName)
    {
    }

    public HomeTypeDetailsModel()
        : this(string.Empty)
    {
    }

    public string? HomeTypeName { get; set; }

    public HousingType HousingType { get; set; }
}
