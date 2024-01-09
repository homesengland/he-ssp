using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record RemoveHomeTypeCommand(string ApplicationId, string HomeTypeId, RemoveHomeTypeAnswer RemoveHomeTypeAnswer) : IRequest<OperationResult>;
