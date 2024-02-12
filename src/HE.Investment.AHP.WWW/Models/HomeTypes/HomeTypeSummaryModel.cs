using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investments.Common.WWW.Models;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class HomeTypeSummaryModel : HomeTypeBasicModel, IEditableViewModel
{
    public HomeTypeSummaryModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public HomeTypeSummaryModel()
        : base(string.Empty, string.Empty)
    {
    }

    public IsSectionCompleted IsSectionCompleted { get; set; }

    public IList<SectionSummaryViewModel>? Sections { get; set; }

    public bool IsEditable { get; set; }

    public bool IsReadOnly => !IsEditable;

    public bool IsApplicationLocked { get; set; }
}
