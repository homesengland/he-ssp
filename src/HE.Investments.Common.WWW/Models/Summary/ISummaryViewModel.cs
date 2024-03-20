namespace HE.Investments.Common.WWW.Models.Summary;

public interface ISummaryViewModel : IEditableViewModel
{
    public IList<SectionSummaryViewModel> Sections { get; }
}
