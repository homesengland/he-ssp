using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.Scheme.Factories;

public interface ISchemeSummaryViewModelFactory
{
    Task<SchemeSummaryViewModel> GetSchemeAndCreateSummary(
        IUrlHelper urlHelper,
        string applicationId,
        CancellationToken cancellationToken);
}
