using HE.Investment.AHP.WWW.Models.Scheme;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.FinancialDetails.Factories;

public interface IFinancialDetailsSummaryModelFactory
{
    Task<FinancialDetailsCheckAnswersModel> GetFinancialDetailsAndCreateSummary(
        IUrlHelper urlHelper,
        Guid applicationId,
        CancellationToken cancellationToken);
}
