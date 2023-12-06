using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class ExemptFromTheRightToSharedOwnershipModel : HomeTypeBasicModel
{
    public ExemptFromTheRightToSharedOwnershipModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public ExemptFromTheRightToSharedOwnershipModel()
        : this(string.Empty, string.Empty)
    {
    }

    public YesNoType ExemptFromTheRightToSharedOwnership { get; set; }
}
