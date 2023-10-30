using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record DuplicateHomeTypeCommand(string ApplicationId, string HomeTypeId) : IRequest<OperationResult<HomeTypeId>>;
