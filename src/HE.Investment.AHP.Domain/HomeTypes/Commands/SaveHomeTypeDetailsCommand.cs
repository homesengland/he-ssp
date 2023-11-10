using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveHomeTypeDetailsCommand(string ApplicationId, string? HomeTypeId, string? HomeTypeName, HousingType HousingType) : IRequest<OperationResult<HomeTypeId?>>;
