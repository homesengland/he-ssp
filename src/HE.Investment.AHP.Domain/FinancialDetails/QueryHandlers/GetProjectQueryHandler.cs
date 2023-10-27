using HE.Investment.AHP.Contract.FinancialDetails.Models;
using HE.Investment.AHP.Contract.FinancialDetails.Queries;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Projects.Queries;
using HE.InvestmentLoans.Contract.Projects.ViewModels;
using MediatR;

namespace HE.Investment.AHP.Domain.FinancialDetails.QueryHandlers;
public class GetFinancialDetailsQueryHandler : IRequestHandler<GetFinancialDetailsQuery, FinancialDetailsViewModel>
{
    private readonly IFinancialDetailsRepository _financialDetailsRepository;

    public GetFinancialDetailsQueryHandler(IFinancialDetailsRepository fianncialDetailsRepository)
    {
        _financialDetailsRepository = fianncialDetailsRepository;
    }

    public async Task<FinancialDetailsViewModel> Handle(GetFinancialDetailsQuery request, CancellationToken cancellationToken)
    {
        var financialDetails = await _financialDetailsRepository.GetById(request.FinancialDetailsId, cancellationToken);

        var schemeName = "Sample schema"; // TODO: implement getting schema name

        return new FinancialDetailsViewModel(
            financialDetails.Id,
            schemeName,
            financialDetails.PurchasePrice?.IsFinal,
            financialDetails.PurchasePrice?.Value);
    }
}
