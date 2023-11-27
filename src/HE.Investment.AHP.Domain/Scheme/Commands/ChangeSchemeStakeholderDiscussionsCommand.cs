using HE.Investment.AHP.Domain.Common;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.Commands;

public record ChangeSchemeStakeholderDiscussionsCommand(string ApplicationId, string? DiscussionReport, FileToUpload? FileToUpload) : IRequest<OperationResult>, IUpdateSchemeCommand;
