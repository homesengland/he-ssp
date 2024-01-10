using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record RemoveHomeTypeCommand(AhpApplicationId ApplicationId, string HomeTypeId, RemoveHomeTypeAnswer RemoveHomeTypeAnswer) : IRequest<OperationResult>;
