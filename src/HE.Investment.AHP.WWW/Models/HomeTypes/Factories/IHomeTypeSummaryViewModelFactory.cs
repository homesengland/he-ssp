using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.WWW.Models.Application;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.HomeTypes.Factories;

public interface IHomeTypeSummaryViewModelFactory
{
    IEnumerable<SectionSummaryViewModel> CreateSummaryModel(FullHomeType homeType, IUrlHelper urlHelper, bool useWorkflowRedirection = false);
}
