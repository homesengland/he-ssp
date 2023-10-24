using HE.Investment.AHP.BusinessLogic.HomeTypes.ValueObjects;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.Investment.AHP.BusinessLogic.HomeTypes.Commands;

public record SaveHousingTypeCommand(string FinancialSchemeId, string? HomeTypeId, string? HomeTypeName, HousingType HousingType) : IRequest<OperationResult<HomeTypeId>>;
