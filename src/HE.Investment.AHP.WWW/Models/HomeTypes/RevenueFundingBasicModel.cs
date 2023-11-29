using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class RevenueFundingBasicModel : HomeTypeBasicModel
{
    public RevenueFundingBasicModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public RevenueFundingBasicModel()
        : this(string.Empty, string.Empty)
    {
    }

    public IList<RevenueFundingSourceType>? Sources { get; set; }
}
