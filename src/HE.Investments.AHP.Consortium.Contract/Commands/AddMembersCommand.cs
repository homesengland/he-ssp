using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.AHP.Consortium.Contract.Commands;

public record AddMembersCommand(ConsortiumId ConsortiumId, AreAllMembersAdded? AreAllMembersAdded) : IRequest<OperationResult>;
