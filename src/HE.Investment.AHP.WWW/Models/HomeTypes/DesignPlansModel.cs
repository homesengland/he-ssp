using HE.Investment.AHP.WWW.Models.Common;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class DesignPlansModel : ProvidedHomeTypeModelBase
{
    public DesignPlansModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
        UploadedFiles = new List<UploadedFileModel>();
    }

    public DesignPlansModel()
        : this(string.Empty, string.Empty)
    {
    }

    public IList<UploadedFileModel> UploadedFiles { get; set; }

    public string MoreInformation { get; set; }
}
