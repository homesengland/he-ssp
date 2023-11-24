using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class MoreInformationModel : ProvidedHomeTypeModelBase
{
    public MoreInformationModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public MoreInformationModel()
        : this(string.Empty, string.Empty)
    {
    }

    public string? MoreInformation { get; set; }
}
