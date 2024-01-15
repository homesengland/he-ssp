using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record DuplicateHomeTypeCommand(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId) : IRequest<OperationResult<HomeTypeId>>;
