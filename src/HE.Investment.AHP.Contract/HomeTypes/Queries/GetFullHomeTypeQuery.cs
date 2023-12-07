using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetFullHomeTypeQuery(string ApplicationId, string HomeTypeId) : IRequest<FullHomeType>;
