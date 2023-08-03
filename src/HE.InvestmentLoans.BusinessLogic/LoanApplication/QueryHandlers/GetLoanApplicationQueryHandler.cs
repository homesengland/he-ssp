using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.QueryHandlers;

public class GetLoanApplicationQueryHandler : IRequestHandler<GetLoanApplicationQuery, GetLoanApplicationQueryResponse>
{
    private readonly ILoanApplicationRepository _loanApplicationRepository;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILoanUserContext _loanUserContext;

    public GetLoanApplicationQueryHandler(ILoanApplicationRepository loanApplicationRepository, ILoanUserContext loanUserContext, IHttpContextAccessor contextAccessor)
    {
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
        _contextAccessor = contextAccessor;
    }

    public async Task<GetLoanApplicationQueryResponse> Handle(GetLoanApplicationQuery request, CancellationToken cancellationToken)
    {
        var loanApplication = await _loanApplicationRepository.GetLoanApplication(request.Id, await _loanUserContext.GetSelectedAccount(), cancellationToken);

        var sessionModel = _contextAccessor.HttpContext?.Session.Get<LoanApplicationViewModel>(request.Id.ToString());

        if (sessionModel != null)
        {
            loanApplication.LegacyModel.UseSectionsFrom(sessionModel);
        }

        loanApplication.LegacyModel.Company.LoanApplicationId = loanApplication.Id.Value;
        return new GetLoanApplicationQueryResponse(loanApplication.LegacyModel);
    }
}
