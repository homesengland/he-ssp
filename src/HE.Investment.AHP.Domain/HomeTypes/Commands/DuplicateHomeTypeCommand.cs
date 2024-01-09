using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record DuplicateHomeTypeCommand(string ApplicationId, string HomeTypeId) : IRequest<OperationResult<HomeTypeId>>;
