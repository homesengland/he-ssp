using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.FinancialDetails.Commands;

public record ProvideLandValueCommand(AhpApplicationId ApplicationId, YesNoType LandOwnership, string? LandValue) : IRequest<OperationResult>;
