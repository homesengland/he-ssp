using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.BusinessLogic.User;

public interface ILoanUserContext
{
    public string UserGlobalId { get; }

    public string Email { get; }

    public IReadOnlyCollection<string> Roles { get; }

    public Task<Guid> GetSelectedAccountId();

    Task<IList<Guid>> GetAllAccountIds();
}
