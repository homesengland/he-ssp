using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetHousingTypeSectionQuery(string SchemeId, string HomeTypeId) : IRequest<HousingTypeSection>;
