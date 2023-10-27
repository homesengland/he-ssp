using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record DuplicateHousingTypeCommand(string SchemeId, string HomeTypeId) : IRequest<OperationResult<HomeTypeId>>;
