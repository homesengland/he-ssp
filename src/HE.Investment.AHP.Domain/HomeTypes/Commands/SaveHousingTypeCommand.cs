using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveHousingTypeCommand(string SchemeId, string? HomeTypeId, string? HomeTypeName, HousingType HousingType) : IRequest<OperationResult<HomeTypeId?>>;
