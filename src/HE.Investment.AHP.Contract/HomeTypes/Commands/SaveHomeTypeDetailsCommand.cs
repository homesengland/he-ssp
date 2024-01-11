using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveHomeTypeDetailsCommand(AhpApplicationId ApplicationId, string? HomeTypeId, string? HomeTypeName, HousingType HousingType) : IRequest<OperationResult<HomeTypeId?>>;
