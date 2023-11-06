using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetHomeTypeQuery(string ApplicationId, string HomeTypeId) : IRequest<HomeType>;
