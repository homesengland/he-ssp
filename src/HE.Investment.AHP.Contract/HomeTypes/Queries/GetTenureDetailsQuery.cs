using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetTenureDetailsQuery(string ApplicationId, string HomeTypeId) : IRequest<TenureDetails>;
