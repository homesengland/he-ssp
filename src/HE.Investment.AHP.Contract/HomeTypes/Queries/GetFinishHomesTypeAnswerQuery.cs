using HE.Investment.AHP.Contract.HomeTypes.Enums;
using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetFinishHomesTypeAnswerQuery(string ApplicationId) : IRequest<FinishHomeTypesAnswer>;
