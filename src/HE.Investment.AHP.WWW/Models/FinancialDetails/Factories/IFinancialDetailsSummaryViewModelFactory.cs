using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.FinancialDetails.Factories;

public interface IFinancialDetailsSummaryViewModelFactory
{
    Task<FinancialDetailsCheckAnswersModel> GetFinancialDetailsAndCreateSummary(
        string applicationId,
        IUrlHelper urlHelper,
        bool isReadOnly,
        CancellationToken cancellationToken);
}
