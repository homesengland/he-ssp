using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.Common.Authorization;

public interface ILoanUserContext
{
    public string UserGlobalId { get; }

    public string? Email { get; }

    public IList<string> Roles { get; }

    public Task<Guid> GetAccountId();
}
