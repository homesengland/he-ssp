using System.Globalization;
using HE.Investment.AHP.Contract.FinancialDetails.Models;
using HE.Investment.AHP.Contract.FinancialDetails.Queries;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using MediatR;

namespace HE.Investment.AHP.Domain.FinancialDetails.QueryHandlers;
public class GetFinancialDetailsQueryHandler : IRequestHandler<GetFinancialDetailsQuery, FinancialDetailsModel>
{
    private readonly IFinancialDetailsRepository _financialDetailsRepository;

    public GetFinancialDetailsQueryHandler(IFinancialDetailsRepository fianncialDetailsRepository)
    {
        _financialDetailsRepository = fianncialDetailsRepository;
    }

    public async Task<FinancialDetailsModel> Handle(GetFinancialDetailsQuery request, CancellationToken cancellationToken)
    {
        var financialDetails = await _financialDetailsRepository.GetById(request.ApplicationId, cancellationToken);

        return new FinancialDetailsModel()
        {
            ApplicationId = financialDetails.ApplicationId.Value,
            ApplicationName = financialDetails.ApplicationName,
            IsPurchasePriceKnown = financialDetails.IsPurchasePriceKnown,
            PurchasePrice = financialDetails.PurchasePrice?.Value.ToString(CultureInfo.InvariantCulture),
            IsSchemaOnPublicLand = (financialDetails.LandOwnership?.Value ?? false) ? CommonResponse.Yes : CommonResponse.No,
            LandValue = financialDetails.LandValue?.Value.ToString(CultureInfo.InvariantCulture),
        };
    }
}
