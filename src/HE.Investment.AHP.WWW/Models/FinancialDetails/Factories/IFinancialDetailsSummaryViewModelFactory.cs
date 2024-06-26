using HE.Investment.AHP.Contract.Application;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.FinancialDetails.Factories;

public interface IFinancialDetailsSummaryViewModelFactory
{
    Task<FinancialDetailsCheckAnswersModel> GetFinancialDetailsAndCreateSummary(
        AhpApplicationId applicationId,
        IUrlHelper urlHelper,
        CancellationToken cancellationToken);
}
