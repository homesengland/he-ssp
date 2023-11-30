using HE.Investment.AHP.WWW.Models.Application;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.Scheme.Factories;

public interface ISchemeSummaryViewModelFactory
{
    SectionSummaryViewModel GetSchemeAndCreateSummary(string title, Contract.Scheme.Scheme scheme, IUrlHelper urlHelper);
}
