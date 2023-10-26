using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetHomeTypeQuery(string SchemeId, string HomeTypeId) : IRequest<HomeType>;
