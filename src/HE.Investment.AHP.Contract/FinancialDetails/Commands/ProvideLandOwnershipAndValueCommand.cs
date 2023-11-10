using HE.Investments.Common.Validators;
using MediatR;
using ApplicationId = HE.Investment.AHP.Contract.FinancialDetails.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Contract.FinancialDetails.Commands;
public record ProvideLandOwnershipAndValueCommand(ApplicationId ApplicationId, string LandOwnership, string LandValue) : IRequest<OperationResult>;
