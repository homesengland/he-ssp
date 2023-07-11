using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.InvestmentLoans.Common.Authorization;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.BusinessLogic.User;

public class LoanUserContext : ILoanUserContext
{
    private readonly IUserContext _userContext;

    private readonly ILoanUserRepository _loanUserRepository;

    private readonly IList<Guid> _accountIds = new List<Guid>();

    private Guid? _selectedAccountId = null;

    public LoanUserContext(IUserContext userContext, ILoanUserRepository loanUserRepository)
    {
        _userContext = userContext;
        _loanUserRepository = loanUserRepository;
    }

    public string UserGlobalId => _userContext.UserGlobalId;

    public string Email => _userContext.Email;

    public IReadOnlyCollection<string> Roles { get; }

    public async Task<Guid> GetSelectedAccountId()
    {
        if (_selectedAccountId is null)
        {
            await LoadUserDetails();
        }

        return _selectedAccountId.Value;
    }

    public async Task<IList<Guid>> GetAllAccountIds()
    {
        if (_selectedAccountId is null)
        {
            await LoadUserDetails();
        }

        return _accountIds;
    }

    private Task LoadUserDetails()
    {
        var userDetails = _loanUserRepository.GetUserDetails(_userContext.UserGlobalId, _userContext.Email);
        _accountIds.AddRange(userDetails?.contactRoles.OrderBy(x => x.accountId).Select(x => x.accountId).ToList());
        _selectedAccountId = _accountIds.FirstOrDefault(Guid.Parse("429d11ab-15fe-ed11-8f6c-002248c653e1"));

        if (_selectedAccountId is null)
        {
            throw new LoanUserAccountIsMissingException();
        }

        return Task.CompletedTask;
    }
}
