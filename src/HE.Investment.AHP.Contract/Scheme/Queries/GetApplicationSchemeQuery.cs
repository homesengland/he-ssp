using HE.Investment.AHP.Contract.Application;
using MediatR;

namespace HE.Investment.AHP.Contract.Scheme.Queries;

public record GetApplicationSchemeQuery(AhpApplicationId ApplicationId, bool IncludeFiles = false) : IRequest<Scheme>;
