using HE.Investment.AHP.Contract.Finance;
using HE.Investment.AHP.Contract.Finance.Queries;
using MediatR;

namespace HE.Investment.AHP.Domain.Finance.QueryHandlers;

public class GetFinancialSchemeQueryHandler : IRequestHandler<GetFinancialSchemeQuery, FinancialScheme>
{
    public Task<FinancialScheme> Handle(GetFinancialSchemeQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new FinancialScheme(request.FinancialSchemeId, "Church road application"));
    }
}
