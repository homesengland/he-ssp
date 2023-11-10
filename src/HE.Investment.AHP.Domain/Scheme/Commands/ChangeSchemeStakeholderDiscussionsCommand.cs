using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.Commands;

public record ChangeSchemeStakeholderDiscussionsCommand(string ApplicationId, string DiscussionReport) : IRequest<OperationResult>, IUpdateSchemeCommand;
