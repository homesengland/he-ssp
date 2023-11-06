using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetDisabledPeopleHomeTypeDetailsQuery(string ApplicationId, string HomeTypeId) : IRequest<DisabledPeopleHomeTypeDetails>;
