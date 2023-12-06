using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.WWW.Models.Application;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class HomeTypeSummaryModel : HomeTypeBasicModel
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
}
