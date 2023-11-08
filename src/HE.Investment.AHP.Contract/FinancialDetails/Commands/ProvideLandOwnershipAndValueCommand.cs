using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.Investment.AHP.Contract.FinancialDetails.Commands;
public record ProvideLandOwnershipAndValueCommand(FinancialDetailsId FinancialDetailsId, string LandOwnership, string LandValue) : IRequest<OperationResult>;
