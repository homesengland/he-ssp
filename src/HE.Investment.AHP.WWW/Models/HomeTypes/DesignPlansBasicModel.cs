using HE.Investments.Common.WWW.Models;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class DesignPlansBasicModel : HomeTypeBasicModel
{
    public DesignPlansBasicModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
        UploadedFiles = new List<FileModel>();
    }

    public DesignPlansBasicModel()
        : this(string.Empty, string.Empty)
    {
    }

    public IList<FileModel> UploadedFiles { get; set; }

    public string? MoreInformation { get; set; }
}
