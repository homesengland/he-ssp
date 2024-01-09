using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.Account.Contract.UserOrganisation.Commands;

public record RemoveLinkBetweenUserAndOrganisationCommand(string UserId) : IRequest<OperationResult>;
