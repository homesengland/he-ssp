using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Scheme.Commands;

public record ChangeSchemeStakeholderDiscussionsCommand(AhpApplicationId ApplicationId, string? DiscussionReport, FileToUpload? FileToUpload) : IRequest<OperationResult>, IUpdateSchemeCommand;
