using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investments.Common.WWW.Models.Summary;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class HomeTypeSummaryModel : HomeTypeBasicModel, ISummaryViewModel
{
    public HomeTypeSummaryModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
        AllowedOperations = new List<AhpApplicationOperation>();
    }

    public HomeTypeSummaryModel()
        : base(string.Empty, string.Empty)
    {
        AllowedOperations = new List<AhpApplicationOperation>();
    }

    public IsSectionCompleted IsSectionCompleted { get; set; }

    public IList<SectionSummaryViewModel> Sections { get; set; }

    public IReadOnlyCollection<AhpApplicationOperation> AllowedOperations { get; set; }

    public bool IsEditable => AllowedOperations.Contains(AhpApplicationOperation.Modification);

    public bool IsReadOnly => !IsEditable;
}
