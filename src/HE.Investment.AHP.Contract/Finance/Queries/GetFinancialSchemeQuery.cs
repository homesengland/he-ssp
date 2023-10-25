using MediatR;

namespace HE.Investment.AHP.Contract.Finance.Queries;

public record GetFinancialSchemeQuery(string FinancialSchemeId) : IRequest<FinancialScheme>;
