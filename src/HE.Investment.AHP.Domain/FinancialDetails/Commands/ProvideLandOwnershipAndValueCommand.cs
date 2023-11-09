using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.FinancialDetails.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.Commands;
public record ProvideLandOwnershipAndValueCommand(ApplicationId ApplicationId, string LandOwnership, string LandValue) : IRequest<OperationResult>;
