using MediatR;

namespace HE.Investment.AHP.Contract.Finance.Queries;

// TODO: change to Application
public record GetFinancialSchemeQuery(string FinancialSchemeId) : IRequest<FinancialScheme>;
