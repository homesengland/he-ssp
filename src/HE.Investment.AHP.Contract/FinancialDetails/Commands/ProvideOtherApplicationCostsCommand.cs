using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.FinancialDetails.Commands;

public record ProvideOtherApplicationCostsCommand(AhpApplicationId ApplicationId, string? ExpectedWorksCosts, string? ExpectedOnCosts) : IRequest<OperationResult>;
