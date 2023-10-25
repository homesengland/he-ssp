using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetHomeTypeQuery(string FinancialSchemeId, string HomeTypeId) : IRequest<HomeType>;
