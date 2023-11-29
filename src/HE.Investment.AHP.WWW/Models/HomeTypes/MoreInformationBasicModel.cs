using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class MoreInformationBasicModel : HomeTypeBasicModel
{
    public MoreInformationBasicModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public MoreInformationBasicModel()
        : this(string.Empty, string.Empty)
    {
    }

    public string? MoreInformation { get; set; }
}
