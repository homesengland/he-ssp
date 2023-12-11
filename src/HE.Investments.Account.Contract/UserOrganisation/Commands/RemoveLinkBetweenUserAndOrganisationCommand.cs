using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investments.Account.Contract.UserOrganisation.Commands;

public record RemoveLinkBetweenUserAndOrganisationCommand() : IRequest<OperationResult>;
