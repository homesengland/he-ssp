using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetHomeTypesQuery(string ApplicationId) : IRequest<IList<HomeTypeDetails>>;
