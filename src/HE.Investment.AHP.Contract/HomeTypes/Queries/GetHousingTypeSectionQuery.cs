using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetHousingTypeSectionQuery(string FinancialSchemeId, string HomeTypeId) : IRequest<HousingTypeSection>;
