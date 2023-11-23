using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveFinishHomeTypesAnswerCommand(string ApplicationId, FinishHomeTypesAnswer FinishHomeTypesAnswer, bool IsCheckOnly = false) : IRequest<OperationResult>;
