using MediatR;

namespace HE.Investment.AHP.Contract.Application.Queries;

public record IsApplicationExistQuery(AhpApplicationId ApplicationId) : IRequest<bool>;
