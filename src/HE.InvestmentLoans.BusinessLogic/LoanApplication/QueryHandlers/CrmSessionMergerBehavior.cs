using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Extensions;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.QueryHandlers;
public class CrmSessionMergerBehavior : IRequestPostProcessor<GetLoanApplicationQuery, GetLoanApplicationQueryResponse>
{
    private readonly IHttpContextAccessor _contextAccessor;

    public CrmSessionMergerBehavior(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public Task Process(GetLoanApplicationQuery request, GetLoanApplicationQueryResponse response, CancellationToken cancellationToken)
    {
        var sessionModel = _contextAccessor.HttpContext?.Session.Get<LoanApplicationViewModel>(request.Id.ToString());

        if (sessionModel != null)
        {
            response.ViewModel.UseSectionsFrom(sessionModel);
        }

        return Task.CompletedTask;
    }
}
