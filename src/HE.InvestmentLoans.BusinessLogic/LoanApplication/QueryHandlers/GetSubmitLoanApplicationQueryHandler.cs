using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Contract.Application.Queries;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.QueryHandlers;

public class GetSubmitLoanApplicationQueryHandler : IRequestHandler<GetSubmitLoanApplicationQuery, GetSubmitLoanApplicationQueryResponse>
{
    private readonly ILoanUserContext _loanUserContext;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILoanApplicationRepository _loanApplicationRepository;

    public GetSubmitLoanApplicationQueryHandler(
        ILoanUserContext loanUserContext,
        ILoanApplicationRepository loanApplicationRepository,
        IHttpContextAccessor contextAccessor)
    {
        _loanUserContext = loanUserContext;
        _loanApplicationRepository = loanApplicationRepository;
        _contextAccessor = contextAccessor;
    }

    public async Task<GetSubmitLoanApplicationQueryResponse> Handle(GetSubmitLoanApplicationQuery request, CancellationToken cancellationToken)
    {
        var selectedAccount = await _loanUserContext.GetSelectedAccount();
        var loanApplication = await _loanApplicationRepository.GetLoanApplication(request.LoanApplicationId, selectedAccount, cancellationToken);
        var sessionModel = _contextAccessor.HttpContext?.Session.Get<LoanApplicationViewModel>(request.LoanApplicationId.ToString());

        if (sessionModel != null)
        {
            loanApplication.LegacyModel.UseSectionsFrom(sessionModel);
        }

        var response = new SubmitLoanApplication(
                            loanApplication.Id,
                            loanApplication.LegacyModel.ReferenceNumber,
                            loanApplication.ExternalStatus,
                            loanApplication.IsEnoughHomesToBuild());

        return new GetSubmitLoanApplicationQueryResponse(response);
    }
}
