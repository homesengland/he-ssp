using HE.Investments.Common.WWW.Models;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class DesignPlansModel : HomeTypeBasicModel
{
    public DesignPlansModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
        UploadedFiles = new List<FileModel>();
    }

    public DesignPlansModel()
        : this(string.Empty, string.Empty)
    {
    }

    public IList<FileModel> UploadedFiles { get; set; }

    public string? MoreInformation { get; set; }

    public int MaxFileSizeInMegabytes { get; set; }

    public string AllowedExtensions { get; set; }
}
