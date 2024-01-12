using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record RemoveHomeTypeCommand(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId, RemoveHomeTypeAnswer RemoveHomeTypeAnswer) : IRequest<OperationResult>;
