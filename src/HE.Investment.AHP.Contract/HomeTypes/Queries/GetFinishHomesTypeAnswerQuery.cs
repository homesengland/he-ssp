using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetFinishHomesTypeAnswerQuery(string ApplicationId) : IRequest<ApplicationHomeTypesFinishAnswer>;
