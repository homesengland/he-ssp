using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetOlderPeopleHomeTypeDetailsQuery(string ApplicationId, string HomeTypeId) : IRequest<OlderPeopleHomeTypeDetails>;
