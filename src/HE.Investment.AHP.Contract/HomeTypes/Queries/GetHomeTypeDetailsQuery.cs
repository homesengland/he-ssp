using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetHomeTypeDetailsQuery(string ApplicationId, string HomeTypeId) : IRequest<HomeTypeDetails>;
