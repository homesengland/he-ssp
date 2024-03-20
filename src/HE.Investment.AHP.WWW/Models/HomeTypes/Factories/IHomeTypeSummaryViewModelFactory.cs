using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investments.Common.WWW.Models.Summary;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.HomeTypes.Factories;

public interface IHomeTypeSummaryViewModelFactory
{
    IEnumerable<SectionSummaryViewModel> CreateSummaryModel(FullHomeType homeType, IUrlHelper urlHelper, bool isReadOnly, bool useWorkflowRedirection = false);
}
