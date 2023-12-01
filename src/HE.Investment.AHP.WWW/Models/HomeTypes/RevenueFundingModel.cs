using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class RevenueFundingModel : HomeTypeBasicModel
{
    public RevenueFundingModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public RevenueFundingModel()
        : this(string.Empty, string.Empty)
    {
    }

    public IList<RevenueFundingSourceType>? Sources { get; set; }
}
