using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Scheme.Commands;

public record ChangeSchemeFundingCommand(AhpApplicationId ApplicationId, string? RequiredFunding, string? HousesToDeliver) : IRequest<OperationResult>, IUpdateSchemeCommand;
