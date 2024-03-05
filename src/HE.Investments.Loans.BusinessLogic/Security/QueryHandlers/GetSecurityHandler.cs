using HE.Investments.Account.Shared;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Loans.BusinessLogic.Security.Repositories;
using HE.Investments.Loans.Contract.Security;
using HE.Investments.Loans.Contract.Security.Queries;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.Security.QueryHandlers;
internal class GetSecurityHandler : IRequestHandler<GetSecurity, GetSecurityResponse>
{
    private readonly ISecurityRepository _securityRepository;
    private readonly IAccountUserContext _loanUserContext;

    public GetSecurityHandler(ISecurityRepository securityRepository, IAccountUserContext loanUserContext)
    {
        _securityRepository = securityRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task<GetSecurityResponse> Handle(GetSecurity request, CancellationToken cancellationToken)
    {
        var security = await _securityRepository.GetAsync(request.Id, await _loanUserContext.GetSelectedAccount(), request.SecurityFieldsSet, cancellationToken);

        return new GetSecurityResponse(
            new SecurityViewModel
            {
                LoanApplicationId = request.Id.Value,
                LoanApplicationStatus = security.LoanApplicationStatus,
                ChargesDebtCompany = security.Debenture?.Exists.MapToCommonResponse(),
                ChargesDebtCompanyInfo = security.Debenture?.Holder,
                DirLoans = security.DirectorLoans?.Exists.MapToCommonResponse(),
                DirLoansSub = security.DirectorLoansSubordinate?.CanBeSubordinated.MapToCommonResponse(),
                DirLoansSubMore = security.DirectorLoansSubordinate?.ReasonWhyCannotBeSubordinated,
                Status = security.Status,
            });
    }
}
