using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Contract.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.CommandHandlers;

public class SubmitLoanApplicationCommandHandler : IRequestHandler<SubmitLoanApplicationCommand>
{
    private readonly ILoanApplicationRepository _loanApplicationRepository;
    private readonly ICanSubmitLoanApplication _canSubmitLoanApplication;

    private readonly ILoanUserContext _loanUserContext;
    private readonly IHttpContextAccessor _contextAccessor;

    public SubmitLoanApplicationCommandHandler(ILoanApplicationRepository loanApplicationRepository, ILoanUserContext loanUserContext, ICanSubmitLoanApplication canSubmitLoanApplication, IHttpContextAccessor contextAccessor)
    {
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
        _canSubmitLoanApplication = canSubmitLoanApplication;
        _contextAccessor = contextAccessor;
    }

    public async Task Handle(SubmitLoanApplicationCommand request, CancellationToken cancellationToken)
    {
        var loanApplication = await _loanApplicationRepository
                                .GetLoanApplication(request.LoanApplicationId, await _loanUserContext.GetSelectedAccount(), cancellationToken);

        var sessionModel = _contextAccessor.HttpContext?.Session.Get<LoanApplicationViewModel>(request.LoanApplicationId.ToString());

        if (sessionModel != null)
        {
            loanApplication.LegacyModel.UseSectionsFrom(sessionModel);
        }

        // Added temporarily until saving every section to crm is implemented
        loanApplication.CheckIfCanBeSubmitted();

        // Added temporarily until saving every section to crm is implemented
        await _loanApplicationRepository.Save(loanApplication.LegacyModel, await _loanUserContext.GetSelectedAccount());

        await loanApplication.Submit(_canSubmitLoanApplication, cancellationToken);
    }
}
