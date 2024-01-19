using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.FinancialDetails.Commands;

public record ProvideLandValueCommand(AhpApplicationId ApplicationId, YesNoType LandOwnership, string? LandValue) : IRequest<OperationResult>;
