using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.InvestmentLoans.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.Common.Authorization;

public class LoanUserContext : ILoanUserContext
{
    private readonly IUserContext _userContext;

    private readonly ILoanUserRepository _loanUserRepository;

    private Guid? _accountId = null;

    public LoanUserContext(IUserContext userContext, ILoanUserRepository loanUserRepository)
    {
        _userContext = userContext;
        _loanUserRepository = loanUserRepository;
    }

    public string UserGlobalId => _userContext.UserGlobalId;

    public string? Email => _userContext.Email;

    public IList<string> Roles { get; private set; }

    public async Task<Guid> GetAccountId()
    {
        if (_accountId is null)
        {
            await LoadUserDetails();
        }

        return _accountId.Value;
    }

    private Task LoadUserDetails()
    {
        var userDetails = _loanUserRepository.GetUserDetails(_userContext.UserGlobalId, _userContext.Email);
        _accountId = userDetails?.contactRoles.OrderBy(x => x.accountId).FirstOrDefault()?.accountId ?? Guid.Parse("429d11ab-15fe-ed11-8f6c-002248c653e1");
        if (_accountId is null)
        {
             throw new LoanUserAccountIsMissingException();
        }

        return Task.CompletedTask;
    }
}
