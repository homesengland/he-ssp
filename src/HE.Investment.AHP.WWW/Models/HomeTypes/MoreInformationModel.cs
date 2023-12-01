namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class MoreInformationModel : HomeTypeBasicModel
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
