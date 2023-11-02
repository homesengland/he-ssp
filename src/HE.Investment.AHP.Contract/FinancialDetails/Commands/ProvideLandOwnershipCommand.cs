using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.Investment.AHP.Contract.FinancialDetails.Commands;
public record ProvideLandOwnershipCommand(FinancialDetailsId FinancialDetailsId, string LandOwnership) : IRequest<OperationResult>;
